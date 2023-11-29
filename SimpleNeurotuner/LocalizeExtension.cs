using SimpleNeurotuner.Resources.Strings;
using Microsoft.Extensions.Localization;

namespace SimpleNeurotuner
{
    [ContentProperty(nameof(Key))]
    public class LocalizeExtension : IMarkupExtension
    {
        IStringLocalizer<Resource> _localizer;

        public string Key { get; set; } = string.Empty;

        public LocalizeExtension()
        {
            _localizer = ServiceHelper.GetService<IStringLocalizer<Resource>>();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            string localizedText = _localizer[Key];
            return localizedText;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}

