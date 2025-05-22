CKEDITOR.plugins.add('boxproducts', {
    icons: 'boxproducts',
    init: function (editor) {
        editor.addCommand('insertBoxProducts', {
            exec: function (editor) {
                var content = '';
                content += '<div class="listProductForNews">';
                content += '<p>Sản phẩm được hiển thị</strong>';
                content += '</div>';

                editor.insertHtml(content);
            }
        });
        editor.ui.addButton('BoxProducts', {
            label: 'Thêm box sản phẩm',
            command: 'insertBoxProducts',
            toolbar: 'insert'
        });
    }
});
