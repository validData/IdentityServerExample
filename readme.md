IdentityServer4 Example
=======================

## Scenario ##

- Two servers:
	- Auth server
		- acts as auth server
			- runs IdentityServer4
			- two APIs: auth and res1
			- two Clients: 
				- client-app
					- used by a typical client app
					- grants: resource owner grant
					- allowed scopes: auth, res1
				- resource-server
					- inter-service communication between resource server 1 and auth server
					- grants: client credentials
					- allowed scopes: auth
		- acts as resource server
	  		- allowed scopes: auth
		  	- audience: auth
	- Resource server 1
		- acts as resource server
			- allowed scopes: res1
		  	- audience: res1
- User authenticates once but needs to get subsequent authentications for subsystems without re-entering credentials
- Resource server 1 needs to check permissions of user when resources are requested



## 1. Initial login ##

POST to http://localhost:60692/connect/token

### Headers ###
Content-Type:application/x-www-form-urlencoded

### Body ###
- grant_type:password
- client_id:client-app
- scope:auth
- username:user1
- password:p@ss


```
POST /connect/token HTTP/1.1
Host: localhost:60692
Content-Type: application/x-www-form-urlencoded

grant_type=password&client_id=client-app&scope=auth&username=user1&password=p%40ss
```

## 2. Subsystem authentication ##

Using access token for subsystem authentication (here: scope `res1` for resource server 1)

POST to http://localhost:60692/connect/token

### Headers ###
Content-Type:application/x-www-form-urlencoded

### Body ###
- grant_type:password
- client_id:client-app
- scope:res1
- username:SUBSYSTEMAUTH
- password:(access token from step 1)


```
POST /connect/token HTTP/1.1
Host: localhost:60692
Content-Type: application/x-www-form-urlencoded

grant_type=password&client_id=client-app&scope=res1&username=SUBSYSTEMAUTH&password=(access token)
```

## 3. Resource access ##

### 3.1 Get app features ###
GET to http://localhost:60692/api/v1/app-features (on auth server)

#### Headers ####
Authorization:Bearer (access token from **step 1**) 

```
GET /api/v1/app-features HTTP/1.1
Host: localhost:60692
Authorization: Bearer (access token from step 1)
```

### 3.2 Actual resource access ###

GET to http://localhost:60709/api/v1/items (on resource server)

Using access token from step 2 to access actual resource.

#### Headers ####
Authorization:Bearer (access token from **step 2**)

```
GET /api/v1/items HTTP/1.1
Host: localhost:60709
Authorization: Bearer (access token from step 2)
```
