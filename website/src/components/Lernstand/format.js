// 'YYYY-MM-DD' -> 'TT.MM.JJJJ'
export function formatDate(iso) {
  const [y, m, d] = iso.split('-');
  return `${d}.${m}.${y}`;
}

function toDate(iso) {
  const [y, m, d] = iso.split('-').map(Number);
  return new Date(y, m - 1, d);
}

// 'YYYY-MM-DD' -> 'TT.MM.'
export function formatShortDate(iso) {
  const [, m, d] = iso.split('-');
  return `${d}.${m}.`;
}

// Short weekday name in the page language, e.g. 'Mo.'.
export function formatWeekday(iso, locale) {
  return new Intl.DateTimeFormat(locale, {weekday: 'short'}).format(toDate(iso));
}

// Month and year in the page language, e.g. 'Okt. 2026'.
export function formatMonth(iso, locale) {
  return new Intl.DateTimeFormat(locale, {month: 'short', year: 'numeric'}).format(toDate(iso));
}
