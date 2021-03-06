using SmartQuant.Data;
using System;
namespace OpenQuant.API.Compression
{
	internal class RangeBarCompressor : BarCompressor
	{
		private const long MULTIPLIER = 10000L;
		protected override void Add(DataEntry entry)
		{
			double price = entry.Items[0].Price;
			if (this.bar == null)
			{
				base.CreateNewBar(global::OpenQuant.API.BarType.Range, entry.DateTime, entry.DateTime, price);
				return;
			}
			base.AddItemsToBar(entry.Items);
			this.bar.bar.EndTime = entry.DateTime;
			bool flag = false;
			while (!flag)
			{
				if (10000.0 * (this.bar.High - this.bar.Low) >= (double)this.newBarSize)
				{
					global::OpenQuant.API.Bar bar = new global::OpenQuant.API.Bar(new SmartQuant.Data.Bar(SmartQuant.Data.BarType.Range, this.newBarSize, entry.DateTime, entry.DateTime, price, price, price, price, 0L, 0L));
					if (this.bar.High == price)
					{
						this.bar.bar.High = this.bar.Low + (double)this.newBarSize / 10000.0;
						this.bar.bar.Close = this.bar.High;
						bar.bar.Low = this.bar.High;
					}
					if (this.bar.Low == price)
					{
						this.bar.bar.Low = this.bar.High - (double)this.newBarSize / 10000.0;
						this.bar.bar.Close = this.bar.Low;
						bar.bar.High = this.bar.Low;
					}
					base.EmitNewCompressedBar();
					this.bar = bar;
					flag = (10000.0 * (this.bar.High - this.bar.Low) < (double)this.newBarSize);
				}
				else
				{
					flag = true;
				}
			}
		}
	}
}
