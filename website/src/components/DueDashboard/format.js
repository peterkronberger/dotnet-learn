// 'YYYY-MM-DD' -> 'TT.MM.JJJJ'
export function formatDate(iso) {
  const [y, m, d] = iso.split('-');
  return `${d}.${m}.${y}`;
}
