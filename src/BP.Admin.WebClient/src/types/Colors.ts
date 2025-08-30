// colors.ts
export const Colors = {
    // Core Brand
    PrimaryBlue: "#039AC8",
    PrimaryDark: "#02688F",
    PrimaryLight: "#6FD6F5",

    // Punchy Supporting
    VividYellow: "#FFD93D",
    BrightPink: "#FF4D6D",
    FreshGreen: "#22C55E",
    BoldPurple: "#9D4EDD",

    // Neutrals
    BackgroundOffWhite: "#FDFDFD",
    DarkGray: "#1E1E1E",
    SoftGray: "#E5E5E5",
    BorderStroke: "#000000",

    // Brand & Variants
    BlueBrand : "#039AC8",        // main category / featured tile
    BlueLightVariant : "#6FD6F5", // secondary tile / hover effect

    // High-Contrast Tiles
    YellowPop : "#FFD93D",   // promotions, calls attention
    PinkEnergy : "#FF4D6D",  // playful, alerts
    PurpleBold : "#9D4EDD",  // secondary accent, discover sections
    GreenFresh : "#22C55E",  // positive states, achievements

    // Neutral Tiles
    OffWhite : "#FDFDFD",    // keeps layout breathable
} as const;

// type of all color keys
export type ColorKey = keyof typeof Colors;

// if you want just values
export type ColorValue = (typeof Colors)[ColorKey];
