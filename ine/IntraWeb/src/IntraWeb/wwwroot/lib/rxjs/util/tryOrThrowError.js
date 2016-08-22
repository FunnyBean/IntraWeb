function tryOrThrowError(target) {
    function tryCatcher() {
        try {
            tryCatcher.target.apply(this, arguments);
        }
        catch (e) {
            throw e;
        }
    }
    tryCatcher.target = target;
    return tryCatcher;
}
exports.tryOrThrowError = tryOrThrowError;
//# sourceMappingURL=tryOrThrowError.js.map