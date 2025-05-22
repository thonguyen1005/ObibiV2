CKEDITOR.plugins.add('tableofcontentnews', {
    icons: 'tableofcontentnews',
    init: function (editor) {
        editor.addCommand('insertTableOfContent', {
            exec: function (editor) {
                var content = '';
                content += '<div class="menufast">';
                content += '<strong>Xem nhanh</strong>';
                content += '<table>';
                content += '<tr class="bold">';
                content += '<td>Icon</td>';
                content += '<td>Tiêu đề</td>';
                content += '<td>Level</td>';
                content += '</tr>';
                content += '<tr>';
                content += '<td>fa-building</td>';
                content += '<td>Menu 1</td>';
                content += '<td>1</td>';
                content += '</tr>';
                content += '</table>';
                content += '</div>';

                editor.insertHtml(content);
            }
        });
        editor.ui.addButton('TableOfContent', {
            label: 'Menu điều hướng',
            command: 'insertTableOfContent',
            toolbar: 'insert'
        });
    }
});
