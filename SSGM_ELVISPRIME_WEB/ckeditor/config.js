/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */
/*
CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
};
*/

CKEDITOR.editorConfig = function( config ) {
	config.toolbarGroups = [
		{ name: 'mode'},
		{ name: 'links' },
		{ name: 'insert' },
		{ name: 'basicstyles', groups: [ 'basicstyles'] },
		'/',
		{ name: 'paragraph', groups: ['align'] },
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'tools' }
	];
	config.allowedContent = true;
	config.font_names = "굴림;돋움;바탕;궁서;굴림체;돋움체;바탕체;궁서체;"+
        'Arial;Comic Sans MS;Courier New;Lucida Sans Unicode;monospace;sans-serif;serif;Tahoma;Times New Roman;Verdana';
};