SemanticImage
=============

该工具用于向图片增加语义信息（通常是TURTLE格式）。  
A tool to add semantic information to images（usually TURTLE format）.  

**PNG**  
向数据末尾增加两个iTxt数据块：语义信息的类型（key="SemanticContentType"，例如值为"turtle"）和具体的语义信息（"SemanticInfo"）。  
Add two iTxt chunk data to end of image: the type of semantic content(key="SemanticContentType", for example the value is "turtle") and the particular semantic content("SemanticInfo").  

如果你在Silverlight中用到了[ImageTools]（http://imagetools.codeplex.com/），可以看考这个讨论：  
If you are using [ImageTools](http://imagetools.codeplex.com/) in Silverlight, please reference this discussion:  
http://imagetools.codeplex.com/discussions/401096  


**SampleData**  
包含一些已经增加了语义信息的图片。  
Include some images already added semantic info.  

**SampleCode201211**  
用到了[pngcs]（http://code.google.com/p/pngcs/）。  
Referenced the project [pngcs](http://code.google.com/p/pngcs/).
