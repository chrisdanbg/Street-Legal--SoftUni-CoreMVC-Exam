$(document).ready(function () {
    if (sessionStorage.getItem('green')) {
        lynxAlert.greenDisplay(sessionStorage.getItem('green'));
        sessionStorage.clear();
    }
    else if (sessionStorage.getItem('red')) {
        lynxAlert.redDisplay(sessionStorage.getItem('red'));
        sessionStorage.clear();
    }
});

var lynxAlert = {

    Green(msg) {
        sessionStorage.setItem('green', msg);
    },

    Red(msg) {
        sessionStorage.setItem('red', msg);
    },

    greenDisplay(msg) {
        let notification = document.createElement("div");
       
        notification.classList.add('notification-body');
        notification.classList.add('lynxShadow');
        notification.setAttribute("id", "nt-bdy");
   
        let notificationmsg = document.createElement("h5");
        notificationmsg.classList.add('notification-msg');
        notificationmsg.setAttribute("id", "nt-msg");
    
        notification.appendChild(notificationmsg);
        document.body.appendChild(notification);

        notification.classList.remove('goBack');
        notification.classList.add('animated');

        notification.classList.add('green');

        notificationmsg.innerHTML = msg;
        setTimeout(function () { animBack(); }, 2000);
    },

    redDisplay(msg) {
        let notification = document.createElement("div");
       
        notification.classList.add('notification-body');
        notification.setAttribute("id", "nt-bdy");

        let notificationmsg = document.createElement("h5");
        notificationmsg.classList.add('notification-msg');
        notificationmsg.setAttribute("id", "nt-msg");

        notification.appendChild(notificationmsg);
        document.body.appendChild(notification);

        notification.classList.remove('goBack');
        notification.classList.add('animated');

        notification.classList.add('red');


        notificationmsg.innerHTML = msg;
        setTimeout(function () { animBack(); }, 2000);
    },
}

function animBack() {
    var notification = document.getElementById('nt-bdy');
    notification.classList.remove('animated');
    notification.classList.add('goBack');
}