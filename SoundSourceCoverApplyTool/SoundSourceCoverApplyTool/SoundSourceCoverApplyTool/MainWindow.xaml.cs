using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SoundSourceCoverApplyTool.Models;
using ParkSquare.Gracenote;
using TagLib.Flac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace SoundSourceCoverApplyTool
{
	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		private const string GRACENOTE_CLIENT_ID = "426862886-B13ACAA631E6302A724410B1BA1DFA63";
		private const string DEFAULT_TAG_IMG_URI = "http://www.gracenote.com/wp-content/uploads/2015/06/favicon.png";
		private string[] m_DeliveredPath = new string[] { };
		private List<string> m_FilesPath = new List<string>();
		private bool m_AllFilesHasTagImage = true;
		// MahApps 전용 변수 정의
		private MetroWindow m_PopupWin;

		public MainWindow()
		{
			InitializeComponent();

			SoundSourceFilePathModel dummy = new SoundSourceFilePathModel();
			dummy.FileName = "Drag & Drop Your Files Here!!";
			dummy.Artist = "Sound Source Cover Apply Tool - Freeware";
			dummy.Title = "Made by AK Cafe - LightEach";
			listBox.ItemsSource = new List<SoundSourceFilePathModel>() { dummy };
		}

		#region ListBoxBind
		private async void ListBoxBind()
		{
			m_AllFilesHasTagImage = true;
			m_FilesPath.Clear();
			listBox.Resources.Clear();

			#region 길어서 접어둠
			m_DeliveredPath.Cast<string>().ToList().ForEach(p =>
			{
				FileAttributes fa = System.IO.File.GetAttributes(p);
				if (fa == FileAttributes.Directory)
				{
					string[] files = Directory.GetFiles(p).Where(s => IsExtensionAllowed(s)).ToArray();
					files.ToList().ForEach(f => { m_FilesPath.Add(f); });
				}
				else
				{
					if (IsExtensionAllowed(p))
						m_FilesPath.Add(p);
				}
			});

			List<SoundSourceFilePathModel> lstSSFPM = new List<SoundSourceFilePathModel>();
			#endregion

			var mySettings = new MetroDialogSettings()
			{
				NegativeButtonText = "Close now",
				AnimateShow = false,
				AnimateHide = false
			};
			var controller = await this.ShowProgressAsync("Sound Source List Update", "Preparing...", settings: mySettings);
			controller.SetIndeterminate();
			controller.SetCancelable(true);
			controller.Maximum = Convert.ToDouble(m_FilesPath.Count);
			double dblCnt = 0.0;

			foreach (string fPath in m_FilesPath)
			{
				SoundSourceFilePathModel ssfpm = new SoundSourceFilePathModel();
				ssfpm.ImageSource = new BitmapImage(new Uri(DEFAULT_TAG_IMG_URI));
				ssfpm.FileName = System.IO.Path.GetFileName(fPath);

				TagLib.File tlFile = TagLib.File.Create(fPath);
				
				ssfpm.Artist = tlFile.Tag.FirstArtist;
				ssfpm.Title = tlFile.Tag.Title;

				int pictureLen = tlFile.Tag.Pictures.Length;
				if (pictureLen > 0)
				{
					TagLib.IPicture pic = tlFile.Tag.Pictures[0];

					MemoryStream ms = new MemoryStream(pic.Data.Data);
					ms.Seek(0, SeekOrigin.Begin);
					BitmapImage bi = new BitmapImage();
					bi.BeginInit();
					bi.StreamSource = ms;
					bi.EndInit();

					ssfpm.ImageSource = bi;
					ssfpm.FileSize = pic.Data.Data.Length;
					ssfpm.FileType = pic.Type + ", " + pic.MimeType;
				}
				else m_AllFilesHasTagImage = false;

				lstSSFPM.Add(ssfpm);

				controller.SetProgress(dblCnt);
				controller.SetMessage("Completed : " + ssfpm.Title);
				dblCnt += 1.0;
				await Task.Delay(50);
			}

			listBox.ItemsSource = lstSSFPM;

			await controller.CloseAsync();

			this.TaskbarItemInfo.Description = "Total Sound Sources : " + lstSSFPM.Count.ToString();
			// 맨 상단에 있는 항목 하나를 선택해준다.
			if (lstSSFPM.Count > 0)
				listBox.SelectedItem = listBox.Items[0];

			SetFooterMsg("ListBox Updated!!", "Count : " + int.Parse(dblCnt.ToString()).ToString());

		}
		#endregion

		#region IsExtensionAllowed
		private bool IsExtensionAllowed(string path)
		{
			string extension = System.IO.Path.GetExtension(path);
			List<string> allowed = new List<string> { ".mp3", ".flac", ".m4a", ".dsf" };
			return allowed.Exists(s => s.Equals(extension));
		}
		#endregion

		#region PopupWindowWithBorder
		public void PopupWindowWithBorder(string title, string text)
		{
			if (m_PopupWin != null)
			{
				m_PopupWin.Close();
			}
			m_PopupWin = new MetroWindow() { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner, Title = title, Width = 500, Height = 300 };
			m_PopupWin.Closed += (o, args) => m_PopupWin = null;
			m_PopupWin.Content = new TextBlock() { Text = text, FontSize = 28, FontWeight = FontWeights.Light, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
			m_PopupWin.BorderThickness = new Thickness(1);
			m_PopupWin.GlowBrush = null;
			m_PopupWin.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
			m_PopupWin.Show();


		}
		#endregion

		#region SetFooterMsg
		private void SetFooterMsg(string major, string minor)
		{
			sbiMajor.Content = major;
			sbiMinor.Content = minor;
		} 
		#endregion

		#region ConfirmBox
		private async Task<MessageDialogResult> ConfirmBox(string title, string content, MessageDialogStyle style)
		{
			MetroDialogSettings mds = new MetroDialogSettings();
			mds.AffirmativeButtonText = "OK";
			mds.NegativeButtonText = "Cancel";
			mds.ColorScheme = MetroDialogOptions.ColorScheme;
			MessageDialogResult result = await this.ShowMessageAsync(title, content, style, mds);
			return result;
		}
		#endregion

		#region ConfirmBoxWithImages : 하다하다 좆같아서 포기했음... 뭐 언젠간 할 수 있겠지...
		//private async void ConfirmBoxWithImages(string title, string content, Stream[] imgStrms)
		//{
		//	BaseMetroDialog dialog = (BaseMetroDialog)this.Resources["CustomDialogTest"];
		//	dialog.Title = title;

		//	await this.ShowMetroDialogAsync(dialog);

		//	Image imgGn = ((Grid)dialog.Content).FindChild<Image>("imgCover");

		//	imgStrms[0].Position = 0;
		//	byte[] byImg = new byte[imgStrms[0].Length];
		//	for (int copiedBytes = 0; copiedBytes < imgStrms[0].Length;)
		//		copiedBytes += imgStrms[0].Read(byImg, copiedBytes, Convert.ToInt32(imgStrms[0].Length) - copiedBytes);

		//	using (MemoryStream ms = new MemoryStream(byImg))
		//	{
		//		ms.Seek(0, SeekOrigin.Begin);
		//		BitmapImage bi = new BitmapImage();
		//		bi.BeginInit();
		//		bi.StreamSource = ms;
		//		bi.EndInit();
		//		imgGn.Source = bi;
		//	}

		//	TextBlock txbContent = ((Grid)dialog.Content).FindChild<TextBlock>("MessageTextBlock");
		//	txbContent.Text = content;
		//	Button btnOk = ((Grid)dialog.Content).FindChild<Button>("btnOk");
		//	btnOk.Click += new RoutedEventHandler(delegate (object sender, RoutedEventArgs e)
		//	{
		//		this.HideMetroDialogAsync(dialog);
		//	});
		//} 
		#endregion

		#region 이벤트 영역

		#region listBox_Drop
		private void listBox_Drop(object sender, DragEventArgs e)
		{
			m_DeliveredPath = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			ListBoxBind();
		}
		#endregion
		
		#region btnRemoveCoverImg_Click
		private async void btnRemoveCoverImg_Click(object sender, RoutedEventArgs e)
		{
			MessageDialogResult result = await ConfirmBox("Are you sure?", "Remove this Sound Source(s)?", MessageDialogStyle.AffirmativeAndNegative);

			if (result == MessageDialogResult.Affirmative)
			{
				foreach (SoundSourceFilePathModel selected in listBox.SelectedItems)
				{
					//SoundSourceFilePathModel selected = (SoundSourceFilePathModel)listBox.SelectedItem;
					if (selected != null)
					{
						string foundFPath = m_FilesPath.FirstOrDefault(p => System.IO.Path.GetFileName(p) == selected.FileName);
						if (foundFPath != null)
						{
							TagLib.File tlFile = TagLib.File.Create(foundFPath);
							tlFile.Tag.Pictures = new TagLib.IPicture[] { };
							tlFile.Save();

							ListBoxBind();

							SetFooterMsg("Cover Image Removed", "Count : 1");
						}
					}
				}

			}
		}
		#endregion

		#region btnApplyInFolderImg_Click
		private async void btnApplyInFolderImg_Click(object sender, RoutedEventArgs e)
		{
			var mySettings = new MetroDialogSettings() { NegativeButtonText = "Close now", AnimateShow = false, AnimateHide = false };

			var controller = await this.ShowProgressAsync("Front Image Changing", "Preparing...", settings: mySettings);
			controller.SetIndeterminate();
			controller.SetCancelable(true);
			controller.Maximum = Convert.ToDouble(m_FilesPath.Count);
			double dblCnt = 0.0;

			foreach (SoundSourceFilePathModel selected in listBox.SelectedItems)
			{
				string fPath = m_FilesPath.FirstOrDefault(p => System.IO.Path.GetFileName(p) == selected.FileName);
				string dir = System.IO.Path.GetDirectoryName(fPath);
				string coverImg = System.IO.Path.Combine(dir, "tag_front.jpg");

				if (System.IO.File.Exists(coverImg))
				{
					using (FileStream fstrm = new FileStream(coverImg, FileMode.Open))
					{
						using (Stream coverStrm = fstrm)
						{
							TagLib.File tlFile = TagLib.File.Create(fPath);
							TagLib.Picture pic = new TagLib.Picture();
							pic.Type = TagLib.PictureType.FrontCover;
							pic.Data = TagLib.ByteVector.FromStream(coverStrm);

							tlFile.Tag.Pictures = new TagLib.IPicture[] { pic };
							tlFile.Save();
						}
					}
				}

				controller.SetProgress(dblCnt);
				controller.SetMessage("Completed : " + System.IO.Path.GetFileName(fPath));
				dblCnt += 1.0;
				await Task.Delay(50);
			}

			await controller.CloseAsync();

			ListBoxBind();

			SetFooterMsg("In Folder Image Apply Complete!!", "Total Sound Sources : " + int.Parse(dblCnt.ToString()).ToString());
		} 
		#endregion

		#region btnDialogTest_Click
		private async void btnDialogTest_Click(object sender, RoutedEventArgs e)
		{
			MetroDialogSettings mds = new MetroDialogSettings();
			mds.AffirmativeButtonText = "OK";
			mds.NegativeButtonText = "Cancel";
			mds.ColorScheme = MetroDialogOptions.ColorScheme;
			MessageDialogResult result = await this.ShowMessageAsync("Sound Source Update...", "15/15 Completed", MessageDialogStyle.AffirmativeAndNegative, mds);
			//if (result != MessageDialogResult.Negative)
			//{

			//}

			//PopupWindowWithBorder("Another Test...", "MetroWindow with Border");
		}
		#endregion

		#region btnApplyCoverImg_Click
		private async void btnApplyCoverImg_Click(object sender, RoutedEventArgs e)
		{
			if (m_AllFilesHasTagImage)
			{
				await ConfirmBox("Action Rejected", "All files are already has tag image", MessageDialogStyle.Affirmative);
				return;
			}

			var mySettings = new MetroDialogSettings() { NegativeButtonText = "Close now", AnimateShow = false, AnimateHide = false };

			var controller = await this.ShowProgressAsync("Front Image Changing", "Preparing...", settings: mySettings);
			controller.SetIndeterminate();
			controller.SetCancelable(true);
			controller.Maximum = Convert.ToDouble(m_FilesPath.Count);
			double dblCnt = 0.0;

			if (listBox.SelectedItems.Count == 0)
			{
				MessageDialogResult mdr = await ConfirmBox("Do you want to continue?", "Proceed for all files", MessageDialogStyle.AffirmativeAndNegative);
				if (mdr == MessageDialogResult.Affirmative)
					listBox.SelectAll();
			}

			foreach (SoundSourceFilePathModel selected in listBox.SelectedItems)
			{
				string fPath = m_FilesPath.FirstOrDefault(p => selected.FileName == System.IO.Path.GetFileName(p));

				if (fPath != null)
				{
					FileInfo fi = new FileInfo(fPath);
					// 파일에 ReadOnly 걸려있으면 없애버린다.
					if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
					{
						FileAttributes attr = fi.Attributes & ~FileAttributes.ReadOnly;

						System.IO.File.SetAttributes(fPath, attr);
					}

					TagLib.File tlFile = TagLib.File.Create(fPath);
					int pictureLen = tlFile.Tag.Pictures.Length;
					if (pictureLen == 0)
					{
						#region Gracenote 에서 이미지 찾아오는 핵심 루틴
						GracenoteClient client = new GracenoteClient(GRACENOTE_CLIENT_ID);
						SearchCriteria sc = new SearchCriteria();
						sc.Artist = tlFile.Tag.FirstArtist;
						sc.TrackTitle = tlFile.Tag.Title;
						sc.SearchMode = SearchMode.BestMatchWithCoverArt;
						sc.SearchOptions = SearchOptions.ArtistImage;
						SearchResult result = client.Search(sc);
						if (result.Status.Code == "OK")
						{
							if (result.Albums.First().Artwork.Count() > 0)
							{
								Uri foundImgUri = result.Albums.First().Artwork.First().Uri;

								using (WebClient wc = new WebClient())
								{
									byte[] btImg = wc.DownloadData(foundImgUri.ToString());
									MemoryStream ms = new MemoryStream(btImg);
									Stream coverStrm = ms;

									TagLib.Picture pic = new TagLib.Picture();
									pic.Type = TagLib.PictureType.FrontCover;
									pic.Data = TagLib.ByteVector.FromStream(coverStrm);

									tlFile.Tag.Pictures = new TagLib.IPicture[] { pic };
									tlFile.Save();

									controller.SetProgress(dblCnt);
									controller.SetMessage("Completed : " + System.IO.Path.GetFileName(fPath));
								}

								dblCnt += 1.0;
							}

							await Task.Delay(50);
						} 
						#endregion
					}
				}
			}

			await controller.CloseAsync();

			ListBoxBind();

			SetFooterMsg("List Binding Complete!!", "Total Sound Sources : " + int.Parse(dblCnt.ToString()).ToString());
		}
		#endregion

		#region StackPanel_MouseLeftButtonUp : 지금은 안쓰지만 참고할 코드가 있어 남겨둠
		//private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		//{
		//	StackPanel sp = (StackPanel)sender;
		//	// 이미지를 가지고 있는 StackPanel을 찾아서 이미지를 가져옴
		//	StackPanel spImg = sp.Children.OfType<StackPanel>().FirstOrDefault();
		//	Image selectedImg = spImg.Children.OfType<Image>().FirstOrDefault();
		//	imgFrontCover.Source = selectedImg.Source;
		//	// 기타 정보들을 가지고 있는 StackPanel을 찾아서 정보들을 가져옴
		//	StackPanel spInfo = sp.Children.OfType<StackPanel>().LastOrDefault();
		//	Label selectedLbl = spInfo.Children.OfType<Label>().FirstOrDefault();
		//	string selectedFn = selectedLbl.Content.ToString();


		//	string findPath = m_FilesPath.FirstOrDefault(p => System.IO.Path.GetFileName(p).Equals(selectedFn));

		//	TagLib.File tlFile = TagLib.File.Create(findPath);
		//	lblArtist.Content = tlFile.Tag.FirstArtist;
		//	lblSongTitle.Content = tlFile.Tag.Title;
		//}
		#endregion

		#region listBox_PreviewKeyDown
		private void listBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				ListBox lb = (ListBox)sender;
				if (lb.SelectedIndex != -1)
				{
					List<SoundSourceFilePathModel> lst = (List<SoundSourceFilePathModel>)listBox.ItemsSource;
					lst.RemoveAt(lb.SelectedIndex);
					listBox.Items.Refresh();

					imgFrontCover.Source = null;
					((TextBlock)lblArtist.FindName("txtArtist")).Text = "";
					((TextBlock)lblSongTitle.FindName("txtSongTitle")).Text = "";
					lblFileSize.Content = "";
					lblType.Content = "";
				}
			}
		}
		#endregion

		#region MetroWindow_KeyDown
		private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				this.Close();
			else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.R)
			{
				btnRemoveCoverImg_Click(null, null);
			}
			else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.F)
			{
				btnApplyInFolderImg_Click(null, null);
			}
			else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.C)
			{
				btnApplyCoverImg_Click(null, null);
			}
			else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.A)
			{
				listBox.SelectAll();
			}
		}
		#endregion

		#region listBox_SelectionChanged : 리스트박스에서 행 하나를 클릭했을때 음원의 각종 정보를 가져와 오른쪽에 보여줌
		private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				SoundSourceFilePathModel ssfpm = (SoundSourceFilePathModel)e.AddedItems[0];
				imgFrontCover.Source = ssfpm.ImageSource;
				((TextBlock)lblArtist.FindName("txtArtist")).Text = ssfpm.Artist;
				((TextBlock)lblSongTitle.FindName("txtSongTitle")).Text = ssfpm.Title;
				lblFileSize.Content = ssfpm.FileSize;
				lblType.Content = ssfpm.FileType;
			}
		}
		#endregion

		#endregion
	}
}
