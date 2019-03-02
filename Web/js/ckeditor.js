function ckEditor(element, url)
{
    CKEDITOR.plugins.addExternal('lineheight', url + '/ckeditor/plugins/lineheight/', 'plugin.js');

    CKEDITOR.replace(element, {
        toolbar: [
                { name: 'source', items: ['Source'] },
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
	            { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-',
                'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
	            },
	            { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
	            { name: 'insert', items: ['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'] },
                //'/',
	            {name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize', 'lineheight'] },
	            { name: 'colors', items: ['TextColor', 'BGColor'] },
	            { name: 'tools', items: ['Maximize', 'ShowBlocks'] }
            ],
        contentsCss: url + '/css/ckeditor.css',
        extraPlugins: 'lineheight',
        font_names: 'Arial/Arial, Helvetica, sans-serif;' +
                        'Calibri;' +
	                    'Comic Sans MS/Comic Sans MS, cursive;' +
	                    'Courier New/Courier New, Courier, monospace;' +
	                    'Georgia/Georgia, serif;' +
	                    'Lucida Sans Unicode/Lucida Sans Unicode, Lucida Grande, sans-serif;' +
	                    'Tahoma/Tahoma, Geneva, sans-serif;' +
	                    'Times New Roman/Times New Roman, Times, serif;' +
	                    'Trebuchet MS/Trebuchet MS, Helvetica, sans-serif;' +
	                    'Verdana/Verdana, Geneva, sans-serif',
        font_defaultLabel: 'Calibri',
        line_height: "10px; 20px; 30px;"
    });
}