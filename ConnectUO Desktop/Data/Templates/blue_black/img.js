// written by Dean Edwards, 2005
// with input from Tino Zijdel, Matthias Miller, Diego Perini

// http://dean.edwards.name/weblog/2005/10/add-event/

function addEvent(element, type, handler) {
	if (element.addEventListener) {
		element.addEventListener(type, handler, false);
	} else {
		if (!handler.$$guid) handler.$$guid = addEvent.guid++;
		if (!element.events) element.events = {};
		var handlers = element.events[type];
		if (!handlers) {
			handlers = element.events[type] = {};
			if (element["on" + type]) {
				handlers[0] = element["on" + type];
			}
		}
		handlers[handler.$$guid] = handler;
		element["on" + type] = handleEvent;
	}
};
addEvent.guid = 1;

function handleEvent(event) {
	var returnValue = true;
	event = event || fixEvent(((this.ownerDocument || this.document || this).parentWindow || window).event);
	var handlers = this.events[event.type];
	for (var i in handlers) {
		this.$$handleEvent = handlers[i];
		if (this.$$handleEvent(event) === false) {
			returnValue = false;
		}
	}
	return returnValue;
};

function fixEvent(event) {
	event.preventDefault = fixEvent.preventDefault;
	event.stopPropagation = fixEvent.stopPropagation;
	return event;
};
fixEvent.preventDefault = function() {
	this.returnValue = false;
};
fixEvent.stopPropagation = function() {
	this.cancelBubble = true;
};

//http://talideon.com/weblog/2005/02/detecting-broken-images-js.cfm
function IsImageOk(img) {
    if (!img.complete) {
        return false;
    }
    if (typeof img.naturalWidth != "undefined" && img.naturalWidth == 0) {
        return false;
    }
    return true;
}
addEvent(window, "load", function() {
    for (var i = 0; i < document.images.length; i++) {
        if (!IsImageOk(document.images[i])) {
			document.images[i].src = "images/spacer.gif";
            //document.images[i].style.visibility = "hidden";
        }
    }
});
