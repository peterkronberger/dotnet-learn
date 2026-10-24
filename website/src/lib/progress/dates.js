const ISO_DATE = /^\d{4}-\d{2}-\d{2}$/;

function pad(n, width) {
  return String(n).padStart(width, '0');
}

function format(date) {
  return `${pad(date.getFullYear(), 4)}-${pad(date.getMonth() + 1, 2)}-${pad(date.getDate(), 2)}`;
}

// Parses 'YYYY-MM-DD' into a local Date, or null if it is not a real calendar date.
function parse(isoDate) {
  if (typeof isoDate !== 'string' || !ISO_DATE.test(isoDate)) return null;
  const [y, m, d] = isoDate.split('-').map(Number);
  const date = new Date(y, m - 1, d);
  date.setFullYear(y); // years 0-99 are otherwise mapped to 1900-1999
  if (date.getFullYear() !== y || date.getMonth() !== m - 1 || date.getDate() !== d) return null;
  return date;
}

// 'YYYY-MM-DD' in local time.
export function toLocalDate(date = new Date()) {
  return format(date);
}

// Calendar arithmetic in local time; independent of DST changes.
export function addDays(isoDate, days) {
  const date = parse(isoDate);
  if (!date) throw new RangeError(`Invalid date: ${isoDate}`);
  return format(new Date(date.getFullYear(), date.getMonth(), date.getDate() + days));
}

export function isValidIsoDate(value) {
  return parse(value) !== null;
}
