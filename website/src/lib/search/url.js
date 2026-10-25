// URL helpers for search. URLs are relative to the site root (without baseUrl).

export function hitUrl(page, anchor) {
  return page.url + (anchor ? `#${anchor}` : '');
}

export function searchPageUrl(query) {
  return `/search?q=${encodeURIComponent(query.trim())}`;
}
