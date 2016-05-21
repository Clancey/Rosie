using System;
using Xamarin.Forms.Platform.iOS;
using Rosie.Resources;
using Xamarin.Forms;
using Rosie.Mobile;
using Rosie.Mobile.iOS;

[assembly: ExportRenderer (typeof (SvgImageView), typeof (SvgImageViewRenderer))]
namespace Rosie.Mobile.iOS
{
	public class SvgImageViewRenderer : ImageRenderer
	{
		public SvgImageViewRenderer ()
		{
			
		}
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == nameof (SvgImageView.Svg))
				LayoutSubviews ();
		}
		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);
			this.LayoutSubviews ();
		}

		CoreGraphics.CGRect lastRect;
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var image = (SvgImageView)Element;
			if (lastRect == Bounds || Control == null || string.IsNullOrWhiteSpace(image?.Svg))
				return;
			lastRect = Bounds;
			var stream = ResourceHelper.GetEmbeddedResourceStream (image.Svg);
			this.Control.LoadSvg (stream);
		}
	}
}

