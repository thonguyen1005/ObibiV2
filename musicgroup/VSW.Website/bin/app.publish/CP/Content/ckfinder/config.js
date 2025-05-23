﻿/*
Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
For licensing, see license.txt or http://cksource.com/ckfinder/license
*/

CKFinder.customConfig = function( config )
{
	// Define changes to default configuration here.
	// For the list of available options, check:
	// http://docs.cksource.com/ckfinder_2.x_api/symbols/CKFinder.config.html

	// Sample configuration options:
	// config.uiColor = '#BDE31E';
	// config.language = 'fr';
	// config.removePlugins = 'basket';
	config.language = 'vi';
	config.removePlugins = 'basket';
	config.defaultSortBy = 'date';
	//config.showContextMenuArrow = true;
	config.rememberLastFolder = true;
	config.chooseFiles = true;
	config.selectMultiple = true;
};
