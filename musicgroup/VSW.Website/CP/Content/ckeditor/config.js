/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */
CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
    config.language = 'vi';
    config.contentsCss = ['/Content/skins/css/bootstrap.min.css', '/Content/skins/css/fontawesome.min.css', '/Content/skins/css/style.css'];
    // config.uiColor = '#AADC6E';
    config.height = '300px';
    config.allowedContent = true;
    //config.extraPlugins = 'video';
    //config.extraPlugins = 'contents,video,tableofcontentnews,boxproducts';
    config.extraPlugins = 'contents,video,boxproducts';
};
