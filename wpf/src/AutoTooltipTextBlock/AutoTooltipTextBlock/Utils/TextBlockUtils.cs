namespace AutoTooltipTextBlock.Utils
{
	using System;
	using System.Windows;
	using System.Windows.Controls;

	public class TextBlockUtils
	{
		#region AutoTooltip attached property

		/// <summary>
		/// Identified the attached AutoTooltip property. When true, this will set the TextBlock TextTrimming
		/// property to WordEllipsis, and display a tooltip with the full text whenever the text is trimmed.
		/// </summary>
		public static readonly DependencyProperty AutoTooltipProperty = DependencyProperty.RegisterAttached(
			"AutoTooltip",
			typeof(bool),
			typeof(TextBlockUtils),
			new PropertyMetadata(false, OnAutoTooltipPropertyChanged));

		/// <summary>
		/// Gets the value of the AutoTooltipProperty dependency property
		/// </summary>
		public static bool GetAutoTooltip(DependencyObject obj)
		{
			return (bool)obj.GetValue(AutoTooltipProperty);
		}

		/// <summary>
		/// Sets the value of the AutoTooltipProperty dependency property
		/// </summary>
		public static void SetAutoTooltip(DependencyObject obj, bool value)
		{
			obj.SetValue(AutoTooltipProperty, value);
		}

		#endregion


		#region Logic

		private static void OnAutoTooltipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBlock textBlock = d as TextBlock;
			if (textBlock == null)
			{
				return;
			}

			if (e.NewValue.Equals(true))
			{
				textBlock.TextTrimming = TextTrimming.WordEllipsis;
				ComputeAutoTooltip(textBlock);
				textBlock.SizeChanged += TextBlock_SizeChanged;
			}
			else
			{
				textBlock.SizeChanged -= TextBlock_SizeChanged;
			}
		}

		private static void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			TextBlock textBlock = sender as TextBlock;
			if (textBlock == null)
			{
				return;
			}

			ComputeAutoTooltip(textBlock);
		}

		/// <summary>
		/// Assigns the ToolTip for the given TextBlock based on whether the text is trimmed
		/// </summary>
		private static void ComputeAutoTooltip(TextBlock textBlock)
		{
			textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			var width = textBlock.DesiredSize.Width;

			if (textBlock.ActualWidth < width)
			{
				ToolTipService.SetToolTip(textBlock, textBlock.Text);
			}
			else
			{
				ToolTipService.SetToolTip(textBlock, null);
			}

		}

		#endregion
	}
}
