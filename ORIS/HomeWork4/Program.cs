using System.Net;
using http_server.Framework.Abstractions.Base;
using http_server.Framework.Abstractions.Interfaces;
using http_server.Framework.Handlers;
using http_server.Framework.Http;
using http_server.Framework.HttpControllers;
using http_server.Framework.Routing;
using http_server.Framework.Services;

HttpListener listener = new HttpListener();

IResourceValidator resourceValidator = new ResourceValidator();
IFileService fileService = FileService.Instance;
Handler fileHandler = new FileHandler(resourceValidator, fileService);

IResourceController resourceController = new ResourceController(resourceValidator, fileService);
Handler controllerHandler = new ControllerHandler(resourceController);

RouteRegistry.Register();
HttpServer myServer = new HttpServer(listener, fileHandler, controllerHandler);


myServer.Run();