using Godot;

namespace Legatus.Scripts.UI;

/// <summary>
/// Centralised Roman-themed UI helpers — palette, fonts, factory methods.
/// Loads the .tres theme from <c>res://ui/ui_theme.tres</c>.
/// </summary>
public static class RomanTheme
{
    // ─── Palette ────────────────────────────────────────────────
    public static readonly Color Parchment      = new(0.82f, 0.77f, 0.63f);
    public static readonly Color ParchmentLight  = new(0.88f, 0.84f, 0.72f);
    public static readonly Color ParchmentDark   = new(0.72f, 0.67f, 0.53f);
    public static readonly Color TextPrimary     = new(0.40f, 0.22f, 0.06f);
    public static readonly Color TextSecondary   = new(0.72f, 0.38f, 0.13f);
    public static readonly Color Gold            = new(0.85f, 0.65f, 0.13f);
    public static readonly Color GoldDark        = new(0.60f, 0.45f, 0.10f);
    public static readonly Color Bronze          = new(0.55f, 0.35f, 0.15f);
    public static readonly Color Positive        = new(0.20f, 0.56f, 0.20f);
    public static readonly Color Negative        = new(0.72f, 0.16f, 0.13f);
    public static readonly Color TooltipBg       = new(0.12f, 0.08f, 0.04f, 0.94f);
    public static readonly Color TooltipTextClr  = new(0.90f, 0.85f, 0.72f);
    public static readonly Color HoverHighlight  = new(0.75f, 0.68f, 0.50f, 0.25f);
    public static readonly Color Border          = new(0.50f, 0.32f, 0.12f);

    // ─── Font sizes ─────────────────────────────────────────────
    public const int FontTitle    = 72;
    public const int FontSubtitle = 44;
    public const int FontBody     = 26;
    public const int FontSmall    = 28;
    public const int FontTooltip  = 24;

    // ─── Cached resources ───────────────────────────────────────
    private static Font  _font;
    private static Theme _theme;

    public static Font  Font  => _font  ??= GD.Load<Font>("res://fonts/Constantine.ttf");
    public static Theme Theme => _theme ??= GD.Load<Theme>("res://ui/ui_theme.tres");

    // ─── LabelSettings helper ───────────────────────────────────

    public static LabelSettings Settings(int size, Color? color = null)
        => new() { Font = Font, FontSize = size, FontColor = color ?? TextPrimary };

    // ─── Label factory ─────────────────────────────────────────

    /// <summary>Creates a themed label with configurable horizontal and vertical alignment.</summary>
    public static Label MakeLabel(
        string text, int size, Color? color = null,
        HorizontalAlignment hAlign = HorizontalAlignment.Center,
        VerticalAlignment vAlign = VerticalAlignment.Top)
    {
        return new Label
        {
            Text                = text,
            LabelSettings       = Settings(size, color),
            HorizontalAlignment = hAlign,
            VerticalAlignment   = vAlign,
            SizeFlagsHorizontal = Control.SizeFlags.Fill
        };
    }

    // ─── Divider ────────────────────────────────────────────────

    public static HSeparator MakeDivider(Color? color = null, int thickness = 2)
    {
        var sep = new HSeparator();
        var sb  = new StyleBoxFlat
        {
            BgColor            = color ?? Bronze,
            ContentMarginTop   = thickness,
            ContentMarginBottom = 0
        };
        sep.AddThemeStyleboxOverride("separator", sb);
        sep.AddThemeConstantOverride("separation", 6);
        return sep;
    }

    // ─── Panel style ────────────────────────────────────────────

    public static StyleBoxFlat PanelStyle(
        Color? bg = null, int border = 2, Color? borderColor = null,
        int radius = 4, float margin = 12f)
    {
        return new StyleBoxFlat
        {
            BgColor                 = bg ?? ParchmentDark,
            BorderWidthTop          = border,
            BorderWidthBottom       = border,
            BorderWidthLeft         = border,
            BorderWidthRight        = border,
            BorderColor             = borderColor ?? Border,
            CornerRadiusTopLeft     = radius,
            CornerRadiusTopRight    = radius,
            CornerRadiusBottomLeft  = radius,
            CornerRadiusBottomRight = radius,
            ContentMarginLeft       = margin,
            ContentMarginRight      = margin,
            ContentMarginTop        = margin,
            ContentMarginBottom     = margin
        };
    }

    // ─── Button factories ───────────────────────────────────────

    public static Button MakeButton(string text, int fontSize = FontBody)
    {
        var btn = new Button { Text = text };
        StyleButton(btn, fontSize);
        return btn;
    }

    public static void StyleButton(Button btn, int fontSize = 40)
    {
        btn.AddThemeFontOverride("font", Font);
        btn.AddThemeFontSizeOverride("font_size", fontSize);
        btn.AddThemeColorOverride("font_color", TextPrimary);
        btn.AddThemeColorOverride("font_hover_color", Gold);
        btn.AddThemeStyleboxOverride("normal",
            PanelStyle(ParchmentDark, 3, GoldDark, 6, 12));
        btn.AddThemeStyleboxOverride("hover",
            PanelStyle(ParchmentLight, 3, Gold, 6, 12));
        btn.AddThemeStyleboxOverride("pressed",
            PanelStyle(new Color(0.65f, 0.60f, 0.48f), 3, Gold, 6, 12));
        btn.AddThemeStyleboxOverride("focus",
            PanelStyle(ParchmentDark, 3, Gold, 6, 12));
    }

    // ─── Tooltip builders ───────────────────────────────────────

    public static PanelContainer TooltipPanel()
    {
        var p = new PanelContainer();
        p.AddThemeStyleboxOverride("panel",
            PanelStyle(TooltipBg, 2, GoldDark, 6, 14));
        return p;
    }

    /// <summary>Left-aligned gold tooltip header.</summary>
    public static Label TooltipHeader(string text)
        => MakeLabel(text, FontSmall, Gold, HorizontalAlignment.Left);

    /// <summary>Left-aligned tooltip body line.</summary>
    public static Label TooltipLine(string text, Color? color = null)
        => MakeLabel(text, FontTooltip, color ?? TooltipTextClr, HorizontalAlignment.Left);

    public static HSeparator TooltipDivider()
        => MakeDivider(new Color(0.6f, 0.5f, 0.3f, 0.5f), 1);
}







