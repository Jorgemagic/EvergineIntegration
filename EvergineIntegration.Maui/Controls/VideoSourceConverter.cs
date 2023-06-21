using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvergineIntegration.Maui.Controls
{
    public class VideoSourceConverter : TypeConverter, IExtendedTypeConverter
    {
        object IExtendedTypeConverter.ConvertFromInvariantString(string value, IServiceProvider serviceProvider)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return VideoSource.FromResource(value);
            }
            throw new InvalidOperationException("Cannot convert null or whitespace to VideoSource.");
        }
    }
}
