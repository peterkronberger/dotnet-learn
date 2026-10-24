export const PROGRESS_KEY = 'dotnet-learn:v1:progress';
export const PROGRESS_EVENT = 'dotnet-learn:progress-changed';
export const LEITNER_INTERVALS_DAYS = [1, 2, 4, 8, 16]; // assumption, adjustable
export const BOX_COUNT = 5;
export const EXPORT_FORMAT = 'dotnet-learn-progress';
export const MAX_IMPORT_LENGTH = 1_000_000;
export const MAX_QUESTION_ID_LENGTH = 100;
export const MAX_LESSON_ID_LENGTH = 40;
// Same pattern as the `lesson` field of quiz questions (src/data/quiz/index.js).
export const LESSON_ID_PATTERN = /^[a-z0-9]+-l\d+$/;

// Display stages for the Leitner boxes (index = box - 1).
// The limits are a decision, not an empirical value – like the intervals above.
export const STAGE_BY_BOX = ['shaky', 'building', 'building', 'solid', 'solid'];
