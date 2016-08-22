var Subscriber_1 = require('../Subscriber');
var rxSubscriber_1 = require('../symbol/rxSubscriber');
function toSubscriber(next, error, complete) {
    if (next && typeof next === 'object') {
        if (next instanceof Subscriber_1.Subscriber) {
            return next;
        }
        else if (typeof next[rxSubscriber_1.rxSubscriber] === 'function') {
            return next[rxSubscriber_1.rxSubscriber]();
        }
        else {
            return new Subscriber_1.Subscriber(next);
        }
    }
    return Subscriber_1.Subscriber.create(next, error, complete);
}
exports.toSubscriber = toSubscriber;
//# sourceMappingURL=toSubscriber.js.map