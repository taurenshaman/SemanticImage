using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

//using SharpDX.Direct2D1;

using Hjg.Pngcs;

namespace SemanticImage.Views {
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainView : MahApps.Metro.Controls.MetroWindow {
    string soureFilePath;
    public const string Key_SemanticInfo = "SemanticInfo";

    public MainView() {
      InitializeComponent();
    }

    private void btnOpen_Click( object sender, RoutedEventArgs e ) {
      CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
      openDialog.ShowPlacesList = true;
      openDialog.Multiselect = false;
      openDialog.IsFolderPicker = false;
      openDialog.AddToMostRecentlyUsedList = true;
      openDialog.Filters.Add( new CommonFileDialogFilter( "PNG images", "*.png" ) );
      if ( openDialog.ShowDialog( this ) == CommonFileDialogResult.Ok ) {
        soureFilePath = openDialog.FileName;
        // get comment meta
        using ( FileStream fileStream = new FileStream( soureFilePath, FileMode.Open, FileAccess.Read ) ) {
          pngReader = new PngReader( fileStream );
          // 参考自Hjg.Pngcs的SampleCustomChunk项目
          // get last line: this forces loading all chunks
          pngReader.ReadChunksOnly();
          tblkComment.Text = pngReader.GetMetadata().GetTxtForKey( Key_SemanticInfo );
          pngReader.End();
          fileStream.Close();
        }

        image.BeginInit();
        image.Source = new BitmapImage( new Uri( soureFilePath ) );
        image.EndInit();
      }
    }

    #region System.Windows.Media.Imaging

    //PngBitmapDecoder decoder;
    //PngBitmapEncoder encoder;

    //    private void btnSave_Click( object sender, RoutedEventArgs e ) {
    //      CommonSaveFileDialog saveDialog = new CommonSaveFileDialog();
    //      saveDialog.ShowPlacesList = true;
    //      saveDialog.AddToMostRecentlyUsedList = true;
    //      saveDialog.Filters.Add( new CommonFileDialogFilter( "PNG images", "*.png" ) );
    //      saveDialog.DefaultFileName = DateTime.Now.ToString( "yyyyMMddhhmmss" ) + ".png";
    //      if ( saveDialog.ShowDialog( this ) == CommonFileDialogResult.Ok ) {
    //        using ( FileStream newFileStream = new FileStream( saveDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite ),
    //          oldFileStream = new FileStream(soureFilePath, FileMode.Open, FileAccess.Read)) {
    //          // get decoder
    //          decoder = PngBitmapDecoder.Create( oldFileStream, BitmapCreateOptions.None, BitmapCacheOption.Default ) as PngBitmapDecoder;
    //          // create encoder
    //          encoder = new PngBitmapEncoder();
    //          // copy
    //          if ( decoder.ColorContexts != null )
    //            encoder.ColorContexts = new System.Collections.ObjectModel.ReadOnlyCollection<ColorContext>( decoder.ColorContexts );
    //          encoder.Interlace = PngInterlaceOption.On;
    //          //if ( decoder.Metadata != null )
    //          //  encoder.Metadata = decoder.Metadata;
    //          //else
    //          //  encoder.Metadata = new BitmapMetadata( "png" );
    //          encoder.Palette = decoder.Palette == null ? BitmapPalettes.Halftone256Transparent : decoder.Palette;
    //          if ( decoder.Preview != null )
    //            encoder.Preview = decoder.Preview;
    //          if ( decoder.Thumbnail != null )
    //            encoder.Thumbnail = decoder.Thumbnail;
    //          // app info
    //          //encoder.Metadata.ApplicationName = "Semantic Image";
    //          // add semantic info
    //          //encoder.Metadata.Subject
    //          //encoder.Metadata.Title
    //          //          encoder.Metadata.Comment = @"
    //          //sky: blue sky.
    //          //heart: cloud heart.
    //          //Meaning: It means love|爱|❤.
    //          //";

    //          foreach ( var frame in decoder.Frames ) {
    //            BitmapMetadata tempMeta = null;
    //            if ( frame.Metadata != null )
    //              tempMeta = (BitmapMetadata)frame.Metadata.Clone();
    //            else
    //              tempMeta = new BitmapMetadata( "png" );
    //            // app info
    //            tempMeta.ApplicationName = "Semantic Image";
    //            // add semantic info
    //            //encoder.Metadata.Subject
    //            //encoder.Metadata.Title
    //            tempMeta.Comment = @"
    //sky: blue sky.
    //heart: cloud heart.
    //Meaning: It means love|爱|❤.
    //            ";

    //            var newframe = BitmapFrame.Create( frame, frame.Thumbnail, tempMeta, frame.ColorContexts );
    //            encoder.Frames.Add( newframe );
    //          }

    //          oldFileStream.Close();

    //          // save to file
    //          encoder.Save( newFileStream );

    //          newFileStream.Close();
    //        }
    //      }
    //    }

    #endregion System.Windows.Media.Imaging


    #region SharpDX

    //    SharpDX.WIC.PngBitmapDecoder decoder;
    //    SharpDX.WIC.PngBitmapEncoder encoder;

    //    private void btnSave_Click( object sender, RoutedEventArgs e ) {
    //      CommonSaveFileDialog saveDialog = new CommonSaveFileDialog();
    //      saveDialog.ShowPlacesList = true;
    //      saveDialog.AddToMostRecentlyUsedList = true;
    //      saveDialog.Filters.Add( new CommonFileDialogFilter( "PNG images", "*.png" ) );
    //      saveDialog.DefaultFileName = DateTime.Now.ToString( "yyyyMMddhhmmss" ) + ".png";
    //      if ( saveDialog.ShowDialog( this ) == CommonFileDialogResult.Ok ) {
    //        using ( FileStream newFileStream = new FileStream( saveDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite ),
    //          oldFileStream = new FileStream(soureFilePath, FileMode.Open, FileAccess.Read)) {
    //          // get decoder
    //            decoder = new SharpDX.WIC.PngBitmapDecoder( new SharpDX.WIC.ImagingFactory() );
    //            decoder.Initialize( new SharpDX.Win32.ComStream( oldFileStream.SafeFileHandle.DangerousGetHandle() ), SharpDX.WIC.DecodeOptions.CacheOnDemand );
    //          // create encoder
    //          encoder = new SharpDX.WIC.PngBitmapEncoder( new SharpDX.WIC.ImagingFactory(), newFileStream );
    //          // copy
    //          if ( decoder.ColorContexts != null )
    //            encoder.SetColorContexts( decoder.ColorContexts );
    //          //encoder.Interlace = PngInterlaceOption.On;
    //          //if ( decoder.Metadata != null )
    //          //  encoder.Metadata = decoder.Metadata;
    //          //else
    //          //  encoder.Metadata = new BitmapMetadata( "png" );
    //          SharpDX.WIC.Palette palette;
    //          decoder.CopyPalette( palette );
    //          encoder.Palette = palette;
    //          if ( decoder.Preview != null )
    //            encoder.Preview = decoder.Preview;
    //          if ( decoder.Thumbnail != null )
    //            encoder.Thumbnail = decoder.Thumbnail;

    //          if ( decoder.FrameCount > 0 ) {
    //            for ( int i = 0; i < decoder.FrameCount; i++ ) {
    //              var frame = decoder.GetFrame( i );

    //            }
    //          }

    //          foreach ( var frame in decoder.Frames ) {
    //            BitmapMetadata tempMeta = null;
    //            if ( frame.Metadata != null )
    //              tempMeta = (BitmapMetadata)frame.Metadata.Clone();
    //            else
    //              tempMeta = new BitmapMetadata( "png" );
    //            // app info
    //            tempMeta.ApplicationName = "Semantic Image";
    //            // add semantic info
    //            //encoder.Metadata.Subject
    //            //encoder.Metadata.Title
    //            tempMeta.Comment = @"
    //sky: blue sky.
    //heart: cloud heart.
    //Meaning: It means love|爱|❤.
    //            ";

    //            var newframe = BitmapFrame.Create( frame, frame.Thumbnail, tempMeta, frame.ColorContexts );
    //            encoder.Frames.Add( newframe );
    //          }

    //          // save to file
    //          encoder.Save( newFileStream );
    //        }
    //      }
    //    }

    #endregion SharpDX


    PngReader pngReader;
    PngWriter pngWriter;

    private void btnSave_Click( object sender, RoutedEventArgs e ) {
      CommonSaveFileDialog saveDialog = new CommonSaveFileDialog();
      saveDialog.ShowPlacesList = true;
      saveDialog.AddToMostRecentlyUsedList = true;
      saveDialog.Filters.Add( new CommonFileDialogFilter( "PNG images", "*.png" ) );
      saveDialog.DefaultFileName = DateTime.Now.ToString( "yyyyMMddhhmmss" ) + ".png";
      if ( saveDialog.ShowDialog( this ) == CommonFileDialogResult.Ok ) {
        using ( FileStream newFileStream = new FileStream( saveDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite ),
          oldFileStream = new FileStream( soureFilePath, FileMode.Open, FileAccess.Read ) ) {
            string semanticInfo = @"
    sky: blue sky.
    heart: cloud heart.
    Meaning: It means love|爱|❤.
                ";

          ////// Reference: http://code.google.com/p/pngcs/wiki/Overview

          // get decoder
          pngReader = new PngReader( oldFileStream );
          // create encoder
          pngWriter = new PngWriter( newFileStream, pngReader.ImgInfo );
          // copy
          int chunkBehav = Hjg.Pngcs.Chunks.ChunkCopyBehaviour.COPY_ALL; // tell to copy all 'safe' chunks
          pngWriter.CopyChunksFirst( pngReader, chunkBehav );          // copy some metadata from reader 
          int channels = pngReader.ImgInfo.Channels;
          if ( channels < 3 )
            throw new Exception( "This example works only with RGB/RGBA images" );
          for ( int row = 0; row < pngReader.ImgInfo.Rows; row++ ) {
            ImageLine l1 = pngReader.ReadRow( row ); // format: RGBRGB... or RGBARGBA...
            pngWriter.WriteRow( l1, row );
          }
          pngWriter.CopyChunksLast( pngReader, chunkBehav ); // metadata after the image pixels? can happen

          // app info
          pngWriter.GetMetadata().SetText( Hjg.Pngcs.Chunks.PngChunkTextVar.KEY_Software, "Semantic Image" );
          // semantic info
          Hjg.Pngcs.Chunks.PngChunk chunk = pngWriter.GetMetadata().SetText( Key_SemanticInfo, semanticInfo, false, false );
          chunk.Priority = true;

          pngWriter.End(); // dont forget this
          pngReader.End();

          oldFileStream.Close();
          newFileStream.Close();
        }
      }
    }

  }

}
