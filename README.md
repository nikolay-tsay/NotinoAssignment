
<h1>Assignment</h1>
Create production-ready ASP.NET Core service app that provides API for storage and retrive documents in different formats
<ul>
<li>The documents are send as a payload of POST request in JSON format and could be modified via PUT verb.</li>
<li>The service is able to return the stored documents in different format, such as XML, MessagePack, etc.</li>
<li>It must be easy to add support for new formats.</li>
<li>It must be easy to add different underlying storage, like cloud, HDD, InMemory, etc.</li>
<li>Assume that the load of this service will be very high (mostly reading).</li>
<li>Demonstrate ability to write unit tests.</li>
<li>The document has mandatory field id, tags and data as shown bellow. The schema of the data field can be arbitrary.</li>
</ul>

<h1>Solution</h1>
<ul>
<li>Controller with 3 endpoints(POST, PUT, GET)</li>
<li>GET endpoint will return document in a format specified in Accept header of the request.</li>
<li>Formatting logic is placed in <b>DocumentsApi/OutputFormatters/DocumentOutputFormatter.cs</b>. </li>
<li>Serializers are made as an extesion methods in <b>DocumentsApi\Extensions\DocumentFormatExtension.cs</b></li>
<li>To add support for a new format:
    <ul>
        <li>Create an extension method in a class above</li>
        <li>Add SupportedMediaType in DocumentOutputFormatter constructor</li>
        <li>Add new format in <b>_serializers</b> dictionary</li>
    </ul>
</li>
<li>Service retrieves and saves documents through IStorage interface, which has to 2 methods
    <ul>
        <li>SaveDocument - to create new document or save changes to existing one</li>
        <li>GetById - to get document by its identifier</li>
    </ul>
</li>
<li>To add another storage option:
    <ul>
        <li>Create new storage class and implement IStorage interface</li>
        <li>Create new value in StorageTypes enum</li>
        <li>Add registration of a new storage in <b>DocumentsApi/Extensions/StartupExtension.cs</b></li>
    </ul>
</li>
<li>StorageType is specified in <b>appsettings.json</b> and can't be changed during runtime</li>
<li>Unit tests are placed in <b>DocumentsTests</b> project and cover logic of <b>DocumentService</b> and existing storages</li>
</ul>
