using MudBlazor;

namespace BlazorSecretManager.Components;

public class CustomTheme
{
    /* 
!
 * Bootswatch v5.3.3 (https://bootswatch.com)
 * Theme: materia
 * Copyright 2012-2024 Thomas Park
 * Licensed under MIT
 * Based on Bootstrap
    */ 
    public static readonly MudTheme Materiatheme = new()
    {
        PaletteLight = new PaletteLight()
        {
            Black = "#000",
            White = "#fff",
            Primary = "#2196f3",
            PrimaryContrastText = "#d3eafd",
            Secondary = "#fff",
            SecondaryContrastText = "rgba(134,134,134,1)",
            Tertiary = "rgba(68, 68, 68, 0.5)",
            TertiaryContrastText = "#f8f9fa",
            Info = "#9c27b0",
            InfoContrastText = "#ebd4ef",
            Success = "#4caf50",
            SuccessContrastText = "#dbefdc",
            Warning = "#ff9800",
            WarningContrastText = "#ffeacc",
            Error = "rgba(244,67,54,1)",
            ErrorContrastText = "rgba(255,255,255,1)",
            Dark = "#222",
            DarkContrastText = "#ced4da",
            TextPrimary = "#0d3c61",
            TextSecondary = "#666666",
            TextDisabled = "rgba(0,0,0,0.3764705882352941)",
            ActionDefault = "rgba(0,0,0,0.5372549019607843)",
            ActionDisabled = "rgba(0,0,0,0.25882352941176473)",
            ActionDisabledBackground = "rgba(0,0,0,0.11764705882352941)",
            Background = "rgba(255,255,255,1)",
            BackgroundGray = "rgba(245,245,245,1)",
            Surface = "rgba(255,255,255,1)",
            DrawerBackground = "white",
            DrawerText = "#fff",
            DrawerIcon = "#fff",
            AppbarBackground = "#d3eafd",
            AppbarText = "#2196f3",
            LinesDefault = "rgba(0,0,0,0.11764705882352941)",
            LinesInputs = "rgba(189,189,189,1)",
            TableLines = "rgba(224,224,224,1)",
            TableStriped = "rgba(0,0,0,0.0196078431372549)",
            TableHover = "rgba(0,0,0,0.0392156862745098)",
            Divider = "rgba(224,224,224,1)",
            DividerLight = "rgba(0,0,0,0.8)",
            PrimaryDarken = "#0d3c61",
            PrimaryLighten = "#d3eafd",
            SecondaryDarken = "#666666",
            SecondaryLighten = "white",
            TertiaryDarken = "rgba(68, 68, 68, 0.5)",
            TertiaryLighten = "rgba(68, 68, 68, 0.5)",
            InfoDarken = "#3e1046",
            InfoLighten = "#ebd4ef",
            SuccessDarken = "#1e4620",
            SuccessLighten = "#dbefdc",
            WarningDarken = "#663d00",
            WarningLighten = "#ffeacc",
            ErrorDarken = "rgb(242,28,13)",
            ErrorLighten = "rgb(246,96,85)",
            DarkDarken = "rgb(46,46,46)",
            DarkLighten = "rgb(87,87,87)",
            HoverOpacity = 0.06,
            RippleOpacity = 0.1,
            RippleOpacitySecondary = 0.2,
            GrayDefault = "#9E9E9E",
            GrayLight = "#BDBDBD",
            GrayLighter = "#E0E0E0",
            GrayDark = "#757575",
            GrayDarker = "#616161",
            OverlayDark = "rgba(33,33,33,0.4980392156862745)",
            OverlayLight = "rgba(255,255,255,0.4980392156862745)",
        },
        PaletteDark = new PaletteDark()
        {
            Black = "#000",
            White = "#fff",
            Primary = "#2196f3",
            PrimaryContrastText = "#071e31",
            Secondary = "#fff",
            SecondaryContrastText = "#333333",
            Tertiary = "rgba(222, 226, 230, 0.5)",
            TertiaryContrastText = "#222222",
            Info = "#9c27b0",
            InfoContrastText = "#1f0823",
            Success = "#4caf50",
            SuccessContrastText = "#0f2310",
            Warning = "#ff9800",
            WarningContrastText = "#331e00",
            Error = "rgba(244,67,54,1)",
            ErrorContrastText = "rgba(255,255,255,1)",
            Dark = "#222",
            DarkContrastText = "#111111",
            TextPrimary = "#7ac0f8",
            TextSecondary = "white",
            TextDisabled = "rgba(255,255,255,0.2)",
            ActionDefault = "rgba(173,173,177,1)",
            ActionDisabled = "rgba(255,255,255,0.25882352941176473)",
            ActionDisabledBackground = "rgba(255,255,255,0.11764705882352941)",
            Background = "rgba(50,51,61,1)",
            BackgroundGray = "rgba(39,39,47,1)",
            Surface = "rgba(55,55,64,1)",
            DrawerBackground = "#333333",
            DrawerText = "#fff",
            DrawerIcon = "#fff",
            AppbarBackground = "#071e31",
            AppbarText = "#2196f3",
            LinesDefault = "rgba(255,255,255,0.11764705882352941)",
            LinesInputs = "rgba(255,255,255,0.2980392156862745)",
            TableLines = "rgba(255,255,255,0.11764705882352941)",
            TableStriped = "rgba(255,255,255,0.2)",
            Divider = "rgba(255,255,255,0.11764705882352941)",
            DividerLight = "rgba(255,255,255,0.058823529411764705)",
            PrimaryDarken = "#7ac0f8",
            PrimaryLighten = "#071e31",
            SecondaryDarken = "white",
            SecondaryLighten = "#333333",
            TertiaryDarken = "rgba(222, 226, 230, 0.5)",
            TertiaryLighten = "rgba(222, 226, 230, 0.5)",
            InfoDarken = "#c47dd0",
            InfoLighten = "#1f0823",
            SuccessDarken = "#94cf96",
            SuccessLighten = "#0f2310",
            WarningDarken = "#ffc166",
            WarningLighten = "#331e00",
            ErrorDarken = "rgb(242,28,13)",
            ErrorLighten = "rgb(246,96,85)",
            DarkDarken = "rgb(23,23,28)",
            DarkLighten = "rgb(56,56,67)",
        },
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "4px",
            DrawerMiniWidthLeft = "56px",
            DrawerMiniWidthRight = "56px",
            DrawerWidthLeft = "240px",
            DrawerWidthRight = "240px",
            AppbarHeight = "64px",
        },
        Typography = new Typography()
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Roboto", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = ".875rem",
                LineHeight = "1.43",
                LetterSpacing = ".01071em",
                TextTransform = "none",
            },
            H1 = new H1Typography
            {
                FontWeight = "300",
                FontSize = "6rem",
                LineHeight = "1.167",
                LetterSpacing = "-.01562em",
                TextTransform = "none",
            },
            H2 = new H2Typography
            {
                FontWeight = "300",
                FontSize = "3.75rem",
                LineHeight = "1.2",
                LetterSpacing = "-.00833em",
                TextTransform = "none",
            },
            H3 = new H3Typography
            {
                FontWeight = "400",
                FontSize = "3rem",
                LineHeight = "1.167",
                LetterSpacing = "0",
                TextTransform = "none",
            },
            H4 = new H4Typography
            {
                FontWeight = "400",
                FontSize = "2.125rem",
                LineHeight = "1.235",
                LetterSpacing = ".00735em",
                TextTransform = "none",
            },
            H5 = new H5Typography
            {
                FontWeight = "400",
                FontSize = "1.5rem",
                LineHeight = "1.334",
                LetterSpacing = "0",
                TextTransform = "none",
            },
            H6 = new H6Typography
            {
                FontWeight = "500",
                FontSize = "1.25rem",
                LineHeight = "1.6",
                LetterSpacing = ".0075em",
                TextTransform = "none",
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontWeight = "400",
                FontSize = "1rem",
                LineHeight = "1.75",
                LetterSpacing = ".00938em",
                TextTransform = "none",
            },
            Subtitle2 = new Subtitle2Typography
            {
                FontWeight = "500",
                FontSize = ".875rem",
                LineHeight = "1.57",
                LetterSpacing = ".00714em",
                TextTransform = "none",
            },
            Body1 = new Body1Typography
            {
                FontWeight = "400",
                FontSize = "1rem",
                LineHeight = "1.5",
                LetterSpacing = ".00938em",
                TextTransform = "none",
            },
            Body2 = new Body2Typography
            {
                FontWeight = "400",
                FontSize = ".875rem",
                LineHeight = "1.43",
                LetterSpacing = ".01071em",
                TextTransform = "none",
            },
            Button = new ButtonTypography
            {
                FontWeight = "500",
                FontSize = ".875rem",
                LineHeight = "1.75",
                LetterSpacing = ".02857em",
                TextTransform = "uppercase",
            },
            Caption = new CaptionTypography
            {
                FontWeight = "400",
                FontSize = ".75rem",
                LineHeight = "1.66",
                LetterSpacing = ".03333em",
                TextTransform = "none",
            },
            Overline = new OverlineTypography
            {
                FontWeight = "400",
                FontSize = ".75rem",
                LineHeight = "2.66",
                LetterSpacing = ".08333em",
                TextTransform = "none",
            },
        },
        ZIndex = new ZIndex()
        {
            Drawer = 1100,
            Popover = 1200,
            AppBar = 1300,
            Dialog = 1400,
            Snackbar = 1500,
            Tooltip = 1600,
        },
    };
    
}