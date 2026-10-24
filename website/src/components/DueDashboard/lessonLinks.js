import {useAllDocsData} from '@docusaurus/plugin-content-docs/client';

// Maps a lesson id ('m01-l03') to the page path, using the docs layout
// convention mXX-<slug>/lYY-<slug> (front matter is not available here).
export function useLessonPath(lessonId) {
  const allData = useAllDocsData();
  const data = allData && allData.default;
  const match = /^(m\d+)-(l\d+)$/.exec(lessonId || '');
  if (!data || !match) return null;
  const version = data.versions.find((v) => v.isLast) ?? data.versions[0];
  if (!version) return null;
  const pattern = new RegExp(`^${match[1]}-[^/]+/${match[2]}-`);
  const doc = version.docs.find((d) => pattern.test(d.id));
  return doc ? doc.path : null;
}
