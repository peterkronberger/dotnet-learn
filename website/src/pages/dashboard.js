import React from 'react';
import Layout from '@theme/Layout';
import {translate} from '@docusaurus/Translate';
import ProgressDashboard from '@site/src/components/ProgressDashboard';

export default function DashboardPage() {
  return (
    <Layout
      title={translate({id: 'lernstand.page.title', message: 'Lernstand'})}
      description={translate({
        id: 'lernstand.page.description',
        message: 'Was heute ansteht, wie weit du im Lernpfad bist und wie sicher der Stoff sitzt.',
      })}>
      <main>
        <ProgressDashboard />
      </main>
    </Layout>
  );
}
