using System.ComponentModel;

namespace ZamZamAPI.Shared.Enum
{
    public enum ServiceCategory
    {
        [Description("Trims & Accessories Manufacturing")]
        TrimsAccessoriesManufacturing = 1,
        [Description("Woven & Care Labels")]
        WovenCareLabels = 2,
        [Description("Hang Tags & Packaging")]
        HangTagsPackaging = 3,
        [Description("Buttons & Zippers")]
        ButtonsZippers = 4,
        [Description("Digital Printing")]
        DigitalPrinting = 5,
        [Description("Paper Embossing")]
        PaperEmbossing = 6,
        [Description("Heat Press & Lamination")]
        HeatPressLamination = 7,
        [Description("Direct Selling (Ready Stock)")]
        DirectSellingReadyStock = 8,
        [Description("Global Sourcing")]
        GlobalSourcing = 9,
        [Description("Other Specific Inquiries")]
        OtherSpecificInquiries = 10
    }
}
