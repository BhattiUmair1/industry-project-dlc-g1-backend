global using FirebaseAdmin;
global using Google.Apis.Auth.OAuth2;
global using Firebase.Auth;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Google.Cloud.Firestore;
global using System.Diagnostics;
global using Swashbuckle.AspNetCore;
global using System.Security.Claims;
global using AuthorizeAttribute = HotChocolate.AspNetCore.Authorization.AuthorizeAttribute;
global using FluentValidation;
global using FluentValidation.AspNetCore;


// Custom
global using DLC.Models;
global using DLC.Repositories;
global using DLC.Services;
// global using DLC.GraphQL;
// global using DLC.Validation;