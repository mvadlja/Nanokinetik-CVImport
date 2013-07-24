/*
**
** Confidential and Proprietary property of Nanokinetik.
** (c) Copyright, Nanokinetik 2013
** All rights reserved.
** May be used only in accordance with the terms and conditions of the
** license agreement governing use of Nanokinetik software
** between you and Nanokinetik or Nanokinetik's authorized reseller.
** Not to be changed without prior written permission of Nanokinetik.
** Any other use is strictly prohibited.
**
** $Revision: 1.0 $
** $Date: 2013/07/02 15:46:02 $
** $Author: mariog $
**
*/
function checkIfLocalFileExists(fullFilePath) {	
	try {
		fullFilePath = getRealLocalFilePath(fullFilePath);
		var fso = null;
		if (window.ActiveXObject) {
		  fso = new ActiveXObject("Scripting.FileSystemObject");
		} else if (window.GeckoActiveXObject) {
		  fso = new GeckoActiveXObject("Scripting.FileSystemObject");
		}    	
    	return (fso.fileExists(fullFilePath));
    } catch (err) {
		alert(initFileSystemObjectErrorMsg + '\n\n' + detailsStr + ':\n ' + err.message);
		return false;
    }
}

function deleteLocalFile(fullFilePath) {
	var fso = null;
	fullFilePath = getRealLocalFilePath(fullFilePath);
	
	try {
		if (window.ActiveXObject) {
		  fso = new ActiveXObject("Scripting.FileSystemObject");
		} else if (window.GeckoActiveXObject) {
		  fso = new GeckoActiveXObject("Scripting.FileSystemObject");
		}
	} catch (err) {
		alert(initFileSystemObjectErrorMsg + '\n\n' + detailsStr + ':\n ' + err.message);
		return false;
	}

	try {
		fso.DeleteFile(fullFilePath, true);
	} catch (err) {
		alert(deleteFileErrorMsg);
		return false;
	}
	
	return true;
}

function initXMLHTTP () {
	var xmlhttp = null;
	try {		
		if (window.ActiveXObject) {
		  xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
		  //xmlhttp = XmlHttp.create();
		} else if (window.XMLHttpRequest) {
		  xmlhttp = new XMLHttpRequest();
		}		   		   		
   	} catch (err) {
		alert(initXMLHTTPErrorMsg + '\n\n' + detailsStr + ':\n' + err.message);
	}
	
	return xmlhttp;
}

function initADOStream (adoType) {
	var adoStream = null;
	try {
		if (window.ActiveXObject) {
			adoStream = new ActiveXObject("ADODB.Stream");
		} else if (window.GeckoActiveXObject) {
			adoStream = new GeckoActiveXObject("ADODB.Stream");
		}   		
   		
   		adoStream.Type = adoType;   		
   	} catch (err) {
		alert(initADOStreamErrorMsg + '\n\n' + detailsStr + ':\n' + err.message);		
	}
	return adoStream;
}

function getContentFromServer (URLstr, xmlHTTPObj) {
	try {
   		xmlHTTPObj.open("GET", URLstr, false);
   	    xmlHTTPObj.send(null);
   	} catch (err) {
		alert(getContentFromServerErrMsg + '\n\n' + detailsStr + ':\n' + err.message);
		return;
	}			
}

function edit(fullFilePath, checkoutURL, openFileAfterDownload){
	var proceed = false;
	fullFilePath = getRealLocalFilePath(fullFilePath);

	if (checkIfLocalFileExists(fullFilePath)) {	  		
  		if (askUserToDeleteLocalFileOnEditAction && confirm(confirmLocalFileDeleteMsg)) {
  			proceed = deleteLocalFile(fullFilePath);
  		} else {
			proceed = false;
			if (openFileAfterDownload) {
				openFileInShell(fullFilePath);
			} else {
				//alert(operationAbortedMsg);
			}
  		}
	} else {  			
		proceed = true;
	}
	
	if (proceed) {
		var xmlhttp = initXMLHTTP();
		getContentFromServer (checkoutURL, xmlhttp);

		try {
			var adoStream = initADOStream(1);
   			adoStream.Open();
   			if (xmlhttp.responseBody != undefined) adoStream.Write(xmlhttp.responseBody);
			adoStream.SaveToFile(fullFilePath, 2); // 2 = adSaveCreateOverWrite
   		} catch (err) {
			alert(writeLocalFileErrorMsg + '\n\n' + detailsStr + ':\n' + err.message);
			return;
		}

		var status = checkIfLocalFileExists(fullFilePath);
		
		if (!status && document.forms['statusForm'] && document.forms['statusForm'].elements['error']) {
			document.forms['statusForm'].elements['error'].value = 'true';
		}

		if (openFileAfterDownload && status) {
			openFileInShell(fullFilePath);
		}
	}			
}

function checkout(fullFilePath, checkoutURL) {
	fullFilePath = getRealLocalFilePath(fullFilePath);
	edit (fullFilePath, checkoutURL, false);      	
}

function openFileInShell(filePath) {
	try {
			filePath = getRealLocalFilePath(filePath);
			var obj = null;
	        if (window.ActiveXObject) 	{
			  obj = new ActiveXObject(activeXForEditAction);
			} else if (window.GeckoActiveXObject) {
			  obj = new GeckoActiveXObject(activeXForEditAction);
			}
		
	  	if (!checkIfLocalFileExists(filePath)) {
	  		alert(localFileMissingErrorMsg + ' (' + filePath + ')');
	  	} else {
			if (activeXForEditAction == "WScript.Shell") {	
				obj.Run('"' + filePath + '"', "1", "false");
			} else {
				obj.ShellExecute('"' + filePath + '"', "", "", "open", "1");					
			}
	  	}
	} catch (err) {
		alert(openLocalFileErrorMsg + '\n\n' + detailsStr + ':\n' + err.message);
		return;
	}
}

function fileUpload(fullFilePath, serverScriptURL)	{
	var adoStream = initADOStream(1);
	fullFilePath = getRealLocalFilePath(fullFilePath);
		
	try {		   		
   		adoStream.Open(); 
   		adoStream.LoadFromFile(fullFilePath);
	} catch (err) {
		alert(loadLocalFileErrorMsg + '\n\n' + detailsStr + ':\n' + err.message);
		return false;
	}		      		   			
	
	try {
		var xmlhttp = initXMLHTTP();
		xmlhttp.open("POST", serverScriptURL, false);
		xmlhttp.send(adoStream.Read(-1));
		adoStream.Close();
	} catch (err) {
		alert(writeContentToServerErrMsg + '\n\n' + detailsStr + ':\n' + err.message);
		return false;			
	}			
	return true;   		
}

function checkStatus(fullFilePath) {	
	if (!checkIfLocalFileExists(fullFilePath)) {
		document.all.saveButton.disabled=true;
	} else {
		document.all.saveButton.disabled=false;
	}
}

function getRealLocalFilePath(localFilePath) {
		if (localFilePath.indexOf('%TEMP%') != -1) {
			var envVariableName = localFilePath.substring(localFilePath.indexOf("%"), localFilePath.lastIndexOf("%")+1);
	  	var wshell = null;
			if (window.ActiveXObject) 	{
			  wshell = new ActiveXObject(activeXForEditAction);
			} else if (window.GeckoActiveXObject) {
			  wshell = new GeckoActiveXObject(activeXForEditAction);
			}	
			var envVariableValue = wshell.ExpandEnvironmentStrings(envVariableName);
			if (envVariableValue != null && envVariableValue != '') {
					localFilePath = localFilePath.replace(envVariableName, envVariableValue);
			}					
		}
		return localFilePath;
}