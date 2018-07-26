// server.js

// BASE SETUP
// =============================================================================

// call the packages we need
var express    = require('express');        // call express
var app        = express();                 // define our app using express
var bodyParser = require('body-parser');
var fs = require("fs");
var apiKey = undefined;

var os = require('os');

//OSX
var zwavePort = '/dev/cu.usbmodem1461';

//Windows
//var zwavePort = '\\\\.\\COM4';

//Raspbery Pi
//var zwavePort = '/dev/ttyACM0';
 
var secrets = require("./Secrets.json");
var OZW = require('openzwave-shared');
var zwave = new OZW({
    Logging: false,
    ConsoleOutput: false,
    Logging: true,
    ConsoleOutput: true,
    NetworkKey: secrets.ZwaveNetworkKey
});

var nodes = [];

zwave.on('driver ready', function(homeid) {
    console.log('scanning homeid=0x%s...', homeid.toString(16));
});


zwave.on('node added', function(nodeid) {
    if(nodeid == null)
        return;
    nodes[nodeid] = {
        manufacturer: '',
        manufacturerid: '',
        product: '',
        producttype: '',
        productid: '',
        type: '',
        name: '',
        loc: '',
        classes: {},
        ready: false,
        nodeid: nodeid
    };
    zwave.refreshNodeInfo(nodeid);
});

zwave.on('value added', function(nodeid, comclass, value) {
    if(nodeid == null)
        return;
    if (!nodes[nodeid]['classes'][comclass])
        nodes[nodeid]['classes'][comclass] = {};
    nodes[nodeid]['classes'][comclass][value.index] = value;
    io.emit('value-added', {
       nodeId:nodeid,
       comclass:comclass,
       value:value, 
    });
});

zwave.on('value changed', function(nodeid, comclass, value) {
    if(nodeid == null)
        return;
    if (nodes[nodeid]['ready']) {
        console.log('node%d: changed: %d:%s:%s->%s', nodeid, comclass,
                value['label'],
                nodes[nodeid]['classes'][comclass][value.index]['value'],
                value['value']);
    }
    if(nodes[nodeid]['classes'][comclass][value.index]['value'] == value['value'])
        return;
    nodes[nodeid]['classes'][comclass][value.index] = value;
    io.emit('value-changed', {
       nodeId:nodeid,
       comclass:comclass,
       value:value, 
    });
});

zwave.on('value removed', function(nodeid, comclass, index) {
    if(nodeid == null)
        return;
    if (nodes[nodeid]['classes'][comclass] &&
        nodes[nodeid]['classes'][comclass][index])
        delete nodes[nodeid]['classes'][comclass][index];
});

zwave.on('node available', function(nodeid, nodeinfo){
    if(nodeid == null)
        return;
    nodes[nodeid]['manufacturer'] = nodeinfo.manufacturer;
    nodes[nodeid]['manufacturerid'] = nodeinfo.manufacturerid;
    nodes[nodeid]['product'] = nodeinfo.product;
    nodes[nodeid]['producttype'] = nodeinfo.producttype;
    nodes[nodeid]['productid'] = nodeinfo.productid;
    nodes[nodeid]['type'] = nodeinfo.type;
    nodes[nodeid]['name'] = nodeinfo.name;
    nodes[nodeid]['loc'] = nodeinfo.loc;
    io.emit('node-available', {
       nodeid:nodeid,
       nodeinfo:nodeinfo 
    });
});

zwave.on('node ready', function(nodeid, nodeinfo) {
    if(nodeid == null)
        return;
    nodes[nodeid]['manufacturer'] = nodeinfo.manufacturer;
    nodes[nodeid]['manufacturerid'] = nodeinfo.manufacturerid;
    nodes[nodeid]['product'] = nodeinfo.product;
    nodes[nodeid]['producttype'] = nodeinfo.producttype;
    nodes[nodeid]['productid'] = nodeinfo.productid;
    nodes[nodeid]['type'] = nodeinfo.type;
    nodes[nodeid]['name'] = nodeinfo.name;
    nodes[nodeid]['loc'] = nodeinfo.loc;
    nodes[nodeid]['ready'] = true;
    console.log('node%d: %s, %s', nodeid,
            nodeinfo.manufacturer ? nodeinfo.manufacturer
                      : 'id=' + nodeinfo.manufacturerid,
            nodeinfo.product ? nodeinfo.product
                     : 'product=' + nodeinfo.productid +
                       ', type=' + nodeinfo.producttype);
    console.log('node%d: name="%s", type="%s", location="%s" productType="%s"', nodeid,
            nodeinfo.name,
            nodeinfo.type,
            nodeinfo.loc,
            nodeinfo.producttype
            );
    for (comclass in nodes[nodeid]['classes']) {
        var sVal = comclass.toString();
        switch (sVal) {
        case '38':// COMMAND_CLASS_SWITCH_BINARY
        case '39': // COMMAND_CLASS_SWITCH_MULTILEVEL
            if(nodeinfo.producttype.toString() != '0x0002'){
                zwave.enablePoll(nodeid, comclass);            
                console.log('Enabled Polling on: node%d:', nodeid);
            }
            break;
        }
        var values = nodes[nodeid]['classes'][comclass];
        console.log('node%d: class %d', nodeid, comclass);
        for (idx in values)
            console.log('node%d:   %s=%s', nodeid, values[idx]['label'], values[idx]['value']);
    }
    
        io.emit('node-ready',nodes[nodeid]);
});

zwave.on('notification', function(nodeid, notif) {
    io.emit('notification', {
        nodeId:nodeid,
        notif:notif
    });
    switch (notif) {
    case 0:
        console.log('node%d: message complete', nodeid);
        break;
    case 1:
        console.log('node%d: timeout', nodeid);
        break;
    case 2:
        console.log('node%d: nop', nodeid);
        break;
    case 3:
        console.log('node%d: node awake', nodeid);
        break;
    case 4:
        console.log('node%d: node sleep', nodeid);
        break;
    case 5:
        console.log('node%d: node dead', nodeid);
        break;
    case 6:
        console.log('node%d: node alive', nodeid);
        break;
        }
});

zwave.on('controller command', function(r,s) {
    console.log('controller commmand feedback: r=%d, s=%d',r,s);
});




zwave.connect(zwavePort);

// configure app to use bodyParser()
// this will let us get the data from a POST
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());


var port = process.env.PORT || 8080;        // set our port

// ROUTES FOR OUR API
// =============================================================================
var router = express.Router();              // get an instance of the express Router

// route middleware to verify a token
router.use(function(req, res, next) {

  // check header or url parameters or post parameters for token
  var token = req.query["apikey"] || req.headers['apikey'];

  // decode token
  if (token) {
      //Load apikey from disk
      if(apiKey == undefined)
      {
        secrets = require("./Secrets.json");
        apiKey = secrets.ApiKey;
      }
      
      if(token == apiKey)
      {  
        next(); 
      }
      else{
          print(token);
          return res.json({ success: false, message: 'Failed to authenticate token.' });   
      }
  } else {

    // if there is no token
    // return an error
    return res.status(403).send({ 
        success: false, 
        message: 'No token provided.' 
    });
    
  }
});

// test route to make sure everything is working (accessed at GET http://localhost:8080/api)
router.get('/', function(req, res) {
    res.json({ message: 'hooray! welcome to our api!' });   
});

router.route('/device')
    .get(function (req,res) {
        var nodeId = req.query['nodeId'];
        if(nodeId == undefined)
        {
            res.json({error:'Query parameter "nodeId" is required'});
            return;
        }
        var node = nodes[nodeId];
        res.json(node);
    })
    .post(function(req, res) {
        var nodeId = req.body.nodeId;
        var commandClass = req.body.commandClass;
        var i = req.body.instance;
        var index = req.body.index;
        var value = req.body.value;
        if(req.body.isRaw)
        {
            value = new Buffer(value);
        }
        zwave.setValue({node_id: nodeId, class_id: commandClass, instance: i, index:index},value);
        res.json({success:true});
    });

router.route('/device/:nodeId')
    .get(function(req,res) {
        var node = nodes[nodeId];
        res.json(node);  
    });
    
router.route('/device/:nodeId/name')
    .get(function(req,res) {
        var nodeId = req.params.nodeId;
        var node = nodes[nodeId];
        res.json({name:node.name});  
    })
     .post(function(req, res) {         
        var nodeId = req.params.nodeId;
        var name = req.body.name;        
        var node = nodes[nodeId];
        node.name = name;
        zwave.setNodeName(nodeId,name);  
        res.json({success:true});
     });
     
     
    
router.route('/device/:nodeId/location')
    .get(function(req,res) {
        var nodeId = req.params.nodeId;
        var node = nodes[nodeId];
        res.json({name:node.loc});  
    })
     .post(function(req, res) {         
        var nodeId = req.params.nodeId;
        var location = req.body.location;        
        var node = nodes[nodeId];
        node.loc = location;
        zwave.setNodeLocation(nodeId,location);  
        zwave.refreshNodeInfo(nodeId);
        res.json({success:true});
     });

router.route('/devices/add')
    .post(function(req,res){
        var secured = req.body.secured;
        if(secured == undefined)
        {
            secured = false;
        }
        zwave.addNode(secured);
        
        res.json({success:true});
    });
  
router.route('/devices/remove')
    .post(function(req,res){     
        zwave.removeNode();        
        res.json({success:true});
    });  


router.route('/devices/:nodeId/switchOn')
    .post(function(req,res){

        var nodeId = req.params.nodeId;
        zwave.setNodeOn(nodeId);
        
        res.json({success:true});
    });

router.route('/devices/:nodeId/switchOff')
    .post(function(req,res){
        
        var nodeId = req.params.nodeId;
        zwave.setNodeOff(nodeId);
        
        res.json({success:true});
    });

router.route('/devices')

    // get all the devices (accessed at GET api/devices)
    .get(function(req, res) { 
        //Sometimes there are undefined nodes.
       res.json(nodes.filter(z=> z != undefined));
    });

// more routes for our API will happen here

// REGISTER OUR ROUTES -------------------------------
// all of our routes will be prefixed with /api
app.use('/api', router);

// START THE SERVER
// =============================================================================


var io = require('socket.io').listen(app.listen(port));

console.log('Magic happens on port ' + port);

process.on('SIGINT', function() {
    console.log('disconnecting...');
    zwave.disconnect(zwavePort);
    process.exit();
});