Rosie Node Server
================

This is an Openzwave node API wrapper. It gives a nice rest api to interact with ZWave devices.

There is also a Socket.IO stream available to get updates in realtime!

Connecting to your Controller
============================
change zwavePort variable to your Controllers port. I have suggested values for different OS versions

Securing the API
===============
If you want to secure the API, uncomment the following code. You can do tru OAuth, or just a generic apiKey.
The rosie client is setup for a generic apiKey


	//route middleware to verify a token
    router.use(function(req, res, next) {

    // check header or url parameters or post parameters for token
    var token = req.query["apikey"] || req.headers['x-access-token'];

    // decode token
    if (token) {

        // verifies secret and checks exp
        // jwt.verify(token, app.get('superSecret'), function(err, decoded) {      
        //   if (err) {
        //     return res.json({ success: false, message: 'Failed to authenticate token.' });    
        //   } else {
            // if everything is good, save to request for use in other routes
            //req.decoded = decoded;    
            next();
        //   }
        // });

    } else {

        // if there is no token
        // return an error
        return res.status(403).send({ 
            success: false, 
            message: 'No token provided.' 
        });
        
    }
    });
