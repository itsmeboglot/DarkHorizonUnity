mergeInto(LibraryManager.library, 
{
	connectToMetamask: function (utf8String) 
	{
		var phraseToSign = UTF8ToString(utf8String);
		ConnectMetamask(phraseToSign);
	},
});
