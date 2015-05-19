namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public enum FaceLandmark
    {
        // Upper left eye brow
        UPPER_LEFT_BROW_RIGHT = 0,
        UPPER_LEFT_BROW_MID_RIGHT = 1,
        UPPER_LEFT_BROW_MID = 2,
        UPPER_LEFT_BROW_MID_LEFT = 3,
        UPPER_LEFT_BROW_LEFT = 4,

        // Upper right eye brow
        UPPER_RIGHT_BROW_LEFT = 5,
        UPPER_RIGHT_BROW_MID_LEFT = 6,
        UPPER_RIGHT_BROW_MID = 7,
        UPPER_RIGHT_BROW_MID_RIGHT = 8,
        UPPER_RIGHT_BROW_RIGHT = 9,

        // Left eye
        LEFT_EYE_RIGHT = 10,
        LEFT_EYE_UPPER_RIGHT = 11,
        LEFT_EYE_UPPER_MID = 12,
        LEFT_EYE_UPPER_LEFT = 13,
        LEFT_EYE_LEFT = 14,
        LEFT_EYE_LOWER_LEFT = 15,
        LEFT_EYE_LOWER_MID = 16,
        LEFT_EYE_LOWER_RIGHT = 17,

        // Right eye
        RIGHT_EYE_LEFT = 18,
        RIGHT_EYE_UPPER_LEFT = 19,
        RIGHT_EYE_UPPER_MID = 20,
        RIGHT_EYE_UPPER_RIGHT = 21,
        RIGHT_EYE_RIGHT = 22,
        RIGHT_EYE_LOWER_RIGHT = 23,
        RIGHT_EYE_LOWER_MID = 24,
        RIGHT_EYE_LOWER_LEFT = 25,

        // Nose
        NOSE_HIGH_CENTER = 26,
        NOSE_UPPER_CENTER = 27,
        NOSE_LOWER_CENTER = 28,
        NOSE_LOW_CENTER = 29,
        NOSE_LEFT_BASE = 30,
        NOSE_MID_BASE = 31,
        NOSE_RIGHT_BASE = 32,

        // Lip
        OUTER_LIP_LEFT = 33,
        OUTER_LIP_UPPER_LEFT = 34,
        OUTER_LIP_UPPER_MID_LEFT = 35,
        OUTER_LIP_UPPER_MID = 36,
        OUTER_LIP_UPPER_MID_RIGHT = 37,
        OUTER_LIP_UPPER_RIGHT = 38,
        OUTER_LIP_RIGHT = 39,

        OUTER_LIP_LOWER_RIGHT = 40,
        OUTER_LIP_LOWER_MID_RIGHT = 41,
        OUTER_LIP_LOWER_MID = 42,
        OUTER_LIP_LOWER_MID_LEFT = 43,
        OUTER_LIP_LOWER_LEFT = 44,

        INNER_LIP_LEFT = 45,
        INNER_LIP_UPPER_MID_LEFT = 46,
        INNER_LIP_UPPER_MID = 47,
        INNER_LIP_UPPER_MID_RIGHT = 48,
        INNER_LIP_RIGHT = 49,

        INNER_LIP_LOWER_MID_RIGHT = 50,
        INNER_LIP_LOWER_MID = 51,
        INNER_LIP_LOWER_MID_LEFT = 52,

        // Face contour
        LEFT_CONTOUR_FAR_UP = 53,
        LEFT_CONTOUR_UP = 54,
        LEFT_CONTOUR_MID_UPPER = 55,
        LEFT_CONTOUR_MID_UP = 56,
        LEFT_CONTOUR_MID_LOW = 57,
        LEFT_CONTOUR_MID_LOWER = 58,
        LEFT_CONTOUR_LOW = 59,
        LEFT_CONTOUR_FAR_LOW = 60,

        MID_CONTOUR = 61,

        RIGHT_CONTOUR_FAR_LOW = 62,
        RIGHT_CONTOUR_LOW = 63,
        RIGHT_CONTOUR_MID_LOWER = 64,
        RIGHT_CONTOUR_MID_LOW = 65,
        RIGHT_CONTOUR_MID_UP = 66,
        RIGHT_CONTOUR_MID_UPPER = 67,
        RIGHT_CONTOUR_UP = 68,
        RIGHT_CONTOUR_FAR_UP = 69,

        // Lower left eye brow
        LOWER_LEFT_BROW_LEFT = 70,
        LOWER_LEFT_BROW_MID = 71,
        LOWER_LEFT_BROW_RIGHT = 72,

        // Lower right eye brow
        LOWER_RIGHT_BROW_LEFT = 73,
        LOWER_RIGHT_BROW_MID = 74,
        LOWER_RIGHT_BROW_RIGHT = 75,

        // Iris
        LEFT_EYE_IRIS = 76,
        RIGHT_EYE_IRIS = 77
    }
}