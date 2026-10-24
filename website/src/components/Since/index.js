import React from 'react';
import Translate, {translate} from '@docusaurus/Translate';
import styles from './styles.module.css';

// Rule 2 (CLAUDE.md): badges only for features since C# 12 / .NET 8.
const MIN_CSHARP = 12;
const MIN_DOTNET = 8;

function isSet(value) {
  return value !== undefined && value !== null && value !== '';
}

function validate(csharp, dotnet) {
  if (!isSet(csharp) && !isSet(dotnet)) {
    throw new Error('<Since>: at least one of "csharp" or "dotnet" is required.');
  }
  const csharpOld = !isSet(csharp) || Number(csharp) < MIN_CSHARP;
  const dotnetOld = !isSet(dotnet) || Number(dotnet) < MIN_DOTNET;
  if (csharpOld && dotnetOld) {
    throw new Error(
      `<Since csharp="${csharp}" dotnet="${dotnet}">: badges are only for features since C# ${MIN_CSHARP} / .NET ${MIN_DOTNET}.`,
    );
  }
}

export default function Since({csharp, dotnet, children}) {
  validate(csharp, dotnet);

  const versions = [];
  if (isSet(csharp)) versions.push(`C# ${csharp}`);
  if (isSet(dotnet)) versions.push(`.NET ${dotnet}`);

  const and = translate({id: 'since.and', message: 'und'});
  const availableLabel = translate(
    {
      id: 'since.availableFrom',
      message: 'Verfügbar ab {versions}',
      description: 'Screen reader text of the version badge',
    },
    {versions: versions.join(` ${and} `)},
  );

  const badge = (
    <span className={styles.badge}>
      <span aria-hidden="true">{versions.join(' · ')}</span>
      <span className={styles.srOnly}>{availableLabel}</span>
    </span>
  );

  if (!children) {
    return badge;
  }

  return (
    <div className={styles.block}>
      <p className={styles.blockBadge}>{badge}</p>
      <details>
        <summary>
          <Translate id="since.before">So schrieb man es vorher</Translate>
        </summary>
        {children}
      </details>
    </div>
  );
}
