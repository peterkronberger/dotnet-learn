// Syntax highlighting overlay for the playground editor (spike S4, ADR 0005).
// A <pre> behind the transparent <textarea> shows the same text, colored per token and per line.
(function () {
    var KEYWORDS = new Set((
        'abstract as base bool break byte case catch char checked class const continue decimal default delegate do double ' +
        'else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is ' +
        'lock long namespace new null object operator out override params private protected public readonly ref return sbyte ' +
        'sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ' +
        'ushort using virtual void volatile while ' +
        'add alias and async await dynamic field file from get global group init into join let managed nameof nint not nuint ' +
        'on or orderby partial record remove required scoped select set unmanaged value var when where with yield'
    ).split(' '));

    // Order matters: the first alternative that matches at a position wins.
    var TOKEN = new RegExp([
        '(\\/\\/[^\\n]*|\\/\\*[\\s\\S]*?(?:\\*\\/|$))',                                   // 1 comment
        '(\\$?@"(?:[^"]|"")*(?:"|$)|@\\$"(?:[^"]|"")*(?:"|$)|"""[\\s\\S]*?(?:"""|$)|\\$?"(?:\\\\.|[^"\\\\\\n])*(?:"|$))', // 2 string
        '(\'(?:\\\\.|[^\'\\\\\\n])*(?:\'|$))',                                           // 3 char
        '(0[xX][0-9a-fA-F_]+|0[bB][01_]+|\\d[\\d_]*(?:\\.\\d[\\d_]*)?(?:[eE][+-]?\\d+)?[fFdDmMuUlL]*)', // 4 number
        '(^[ \\t]*#[^\\n]*)',                                                           // 5 preprocessor
        '([A-Za-z_][A-Za-z0-9_]*)'                                                      // 6 identifier
    ].join('|'), 'gm');

    function escapeHtml(text) {
        return text.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }

    // Returns one HTML string per source line. Tokens that span lines (block comments, raw strings) are split.
    function highlightLines(code) {
        var lines = [''];
        function push(text, cls) {
            var parts = text.split('\n');
            for (var i = 0; i < parts.length; i++) {
                if (i > 0) { lines.push(''); }
                if (parts[i] === '') { continue; }
                var html = escapeHtml(parts[i]);
                lines[lines.length - 1] += cls ? '<span class="tok-' + cls + '">' + html + '</span>' : html;
            }
        }

        var last = 0;
        var match;
        TOKEN.lastIndex = 0;
        while ((match = TOKEN.exec(code)) !== null) {
            if (match[0] === '') { TOKEN.lastIndex++; continue; }
            push(code.slice(last, match.index), null);
            var cls = match[1] !== undefined ? 'comment'
                : match[2] !== undefined || match[3] !== undefined ? 'string'
                : match[4] !== undefined ? 'number'
                : match[5] !== undefined ? 'meta'
                : KEYWORDS.has(match[6]) ? 'keyword' : null;
            push(match[0], cls);
            last = match.index + match[0].length;
        }
        push(code.slice(last), null);
        return lines;
    }

    var state = { errors: [], warnings: [], currentLine: 0 };

    function lineOf(textarea) {
        if (document.activeElement !== textarea) { return 0; }
        return textarea.value.slice(0, textarea.selectionStart).split('\n').length;
    }

    function render() {
        var textarea = document.getElementById('editor');
        var pre = document.getElementById('editor-highlight');
        if (!textarea || !pre) { return; }

        var lines = highlightLines(textarea.value);
        var html = '';
        for (var i = 0; i < lines.length; i++) {
            var n = i + 1;
            var cls = 'pg-line';
            if (state.errors.indexOf(n) >= 0) { cls += ' pg-line-err'; }
            else if (state.warnings.indexOf(n) >= 0) { cls += ' pg-line-warn'; }
            if (n === state.currentLine) { cls += ' pg-line-current'; }
            html += '<span class="' + cls + '">' + lines[i] + '</span>';
        }
        pre.innerHTML = html;
        pre.parentElement.classList.add('pg-overlay');
        syncScroll();
    }

    function syncScroll() {
        var textarea = document.getElementById('editor');
        var pre = document.getElementById('editor-highlight');
        if (textarea && pre) {
            pre.scrollTop = textarea.scrollTop;
            pre.scrollLeft = textarea.scrollLeft;
        }
    }

    function updateCurrentLine() {
        var textarea = document.getElementById('editor');
        if (!textarea) { return; }
        var line = lineOf(textarea);
        if (line !== state.currentLine) {
            state.currentLine = line;
            render();
        }
    }

    if (typeof document !== 'undefined') {
        window.pgEditor = {
            refresh: function (errors, warnings) {
                state.errors = errors || [];
                state.warnings = warnings || [];
                var textarea = document.getElementById('editor');
                state.currentLine = textarea ? lineOf(textarea) : 0;
                render();
            }
        };

        document.addEventListener('input', function (event) {
            if (event.target && event.target.id === 'editor') {
                // Edited lines no longer match the compiler's line numbers.
                state.errors = [];
                state.warnings = [];
                state.currentLine = lineOf(event.target);
                render();
            }
        });
        document.addEventListener('scroll', function (event) {
            if (event.target && event.target.id === 'editor') { syncScroll(); }
        }, true);
        document.addEventListener('selectionchange', updateCurrentLine);
        document.addEventListener('focusin', updateCurrentLine);
        document.addEventListener('focusout', function () { setTimeout(updateCurrentLine, 0); });
    }

    if (typeof module !== 'undefined') { module.exports = { highlightLines: highlightLines }; }
})();
