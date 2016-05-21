﻿using System;
using Rosie.Mobile;
using Rosie.Mobile.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer (typeof (SvgImageView), typeof (SvgImageViewRenderer))]
namespace Rosie.Mobile.Droid
{
	public class SvgImageViewRenderer : ImageRenderer
	{
		public SvgImageViewRenderer ()
		{
			
		}
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			switch (e.PropertyName) {
			case nameof (SvgImageView.Svg):
				DrawImage ();
				return;
			}
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);
			if (e.NewElement != null)
				DrawImage ();
		}

		double lastWidth;
		double lastHeight;
		async void DrawImage ()
		{
			var svgImage = (SvgImageView)Element;
			var width = Element.WidthRequest;
			var height = Element.HeightRequest;
			bool cached = svgImage.CacheImages;

			if (Math.Abs (width) < double.Epsilon || Math.Abs (height) < double.Epsilon ||
			    (Math.Abs (width - lastWidth) < double.Epsilon && Math.Abs (height - lastHeight) < double.Epsilon) ) {
				return;
			}
			lastWidth = width;
			lastHeight = height;
			if (cached) {
				Control.LoadSvg (svgImage.Svg, new NGraphics.Size (width, height));
			}

			var imagePath = Images.GetCachedImagePath (svgImage.Svg, Math.Max (width, height));
			var image = await Drawable.CreateFromPathAsync (imagePath);
			Control.SetImageDrawable (image);
		}
	}
}

