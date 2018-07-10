// removeClass: ng-show

(function () {
    l2lApp.animation(".ng-anim-slide-down", [function () {
        return {
            enter: function (element, doneFn) {
                element.css("display", "none");
                jQuery(element).slideDown(500, doneFn);
            },

            move: function (element, doneFn) {
                //element.css("display", "block");
                //jQuery(element).show("slide", { direction: "right" }, 700, doneFn);
            },
            leave: function (element, doneFn) {
                element.css("display", "none");
                //jQuery(element).fadeOut(500, doneFn);
            },
            addClass: function (element, className, doneFn) {
                jQuery(element).slideUp(500, doneFn);
            },
            removeClass: function (element, className, doneFn) {
                element.css("display", "none");
                jQuery(element).slideDown(500, doneFn);
            },
            setClass: function (element, addedClass, removedClass, doneFn) {
                jQuery(element).slideDown(1000, doneFn);
            }
        }
    }]);

    l2lApp.animation(".ng-anim-slide-left", [function () {
        return {
            enter: function (element, doneFn) {
                element.css("display", "none");
                //jQuery(element).fadeIn(500, doneFn);
                jQuery(element).show("slide", { direction: "right" }, 700 , doneFn);

                // remember to call doneFn so that angular
                // knows that the animation has concluded
            },

            move: function (element, doneFn) {
                //element.css("display", "block");
                //jQuery(element).show("slide", { direction: "right" }, 700, doneFn);
            },
            leave: function (element, doneFn) {
                //element.css("display", "block");
                jQuery(element).show("slide", { direction: "right" }, 700, doneFn);
                //jQuery(element).fadeOut(500, doneFn);
            },
            addClass: function (element, className, doneFn) {
                jQuery(element).slideUp(500, doneFn);
            },
            removeClass: function (element, className, doneFn) {
                element.css("display", "none");
                jQuery(element).slideDown(500, doneFn);
            },
            setClass: function (element, addedClass, removedClass, doneFn) {
                jQuery(element).slideDown(1000, doneFn);
            }
        }
    }]);

    l2lApp.animation(".ng-anim-slide-right", [function () {
        return {
            enter: function (element, doneFn) {
                element.css("display", "none");
                //jQuery(element).show("slide", { direction: "left" }, 700, doneFn);
            },
            move: function (element, doneFn) {
            },
            leave: function (element, doneFn) {
                //jQuery(element).show("slide", { direction: "left" }, 700, doneFn);
            },
            addClass: function (element, className, doneFn) {
                //jQuery(element).show("slide", { direction: "right" }, 700, doneFn);
                jQuery(element).toggle("slide", {}, 700, doneFn);
            },
            removeClass: function (element, className, doneFn) {
                element.css("display", "none");
                //jQuery(element).show("slide", { direction: "left" }, 700, doneFn);
                jQuery(element).toggle("slide", {}, 700, doneFn);
            },
            setClass: function (element, addedClass, removedClass, doneFn) {
                //jQuery(element).show("slide", { direction: "left" }, 700, doneFn);
            }
        }
    }]);

    l2lApp.animation(".ng-anim-fade", [function () {
        return {
            enter: function (element, doneFn) {
                element.css("display", "none");
                jQuery(element).fadeIn(500, doneFn);
                // remember to call doneFn so that angular
                // knows that the animation has concluded
            },

            move: function (element, doneFn) {
                //element.css("display", "block");
                //jQuery(element).show("slide", { direction: "right" }, 700, doneFn);
            },
            leave: function (element, doneFn) {
                element.css("display", "none");
                //jQuery(element).fadeOut(500, doneFn);
            },
            addClass: function (element, className, doneFn) {
               jQuery(element).fadeOut(500, doneFn);
            },
            removeClass: function (element, className, doneFn) {
                element.css("display", "none");
                jQuery(element).fadeIn(500, doneFn);
            },
            setClass: function (element, addedClass, removedClass, doneFn) {
                //jQuery(element).slideDown(1000, doneFn);
            }
        }
    }]);
})();