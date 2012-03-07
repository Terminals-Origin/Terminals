
CSC  =	mcs

FILES = RssFeed.cs	\
	RssModule.cs	\
	RssModuleItem.cs	\
	RssReader.cs	\
	RssWriter.cs	\
	Collections/ExceptionCollection.cs \
	Collections/RssCategoryCollection.cs \
	Collections/RssChannelCollection.cs \
	Collections/RssFeedCollection.cs \
	Collections/RssItemCollection.cs	\
	Collections/RssModuleCollection.cs	\
	Collections/RssModuleItemCollection.cs	\
	Collections/RssModuleItemCollectionCollection.cs	\
	RssChannel/RssChannel.cs	\
	RssChannel/RssCloud.cs	\
	RssChannel/RssImage.cs	\
	RssChannel/RssTextInput.cs	\
	RssItem/RssEnclosure.cs	\
	RssItem/RssGuid.cs	\
	RssItem/RssItem.cs	\
	RssItem/RssSource.cs	\
	RssModules/RssPhotoAlbum.cs \
	Shared/DBBool.cs	\
	Shared/RssCategory.cs	\
	Shared/RssDefault.cs	\
	Shared/RssElement.cs	\
	Shared/RssEnumerators.cs

	

all:
	$(CSC) /target:library $(FILES) -o RSS.NET.dll
	
clean: 
	rm -f *.exe
	rm -f *.so
	rm -f *.dll
