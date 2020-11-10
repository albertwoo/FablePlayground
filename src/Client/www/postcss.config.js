const purgecss = require('@fullhuman/postcss-purgecss')

module.exports = {
    plugins: [
        purgecss({
            // Specify the paths to all of the template files in your project
            whitelist: ['body'],
            content: [
                '../**/*.fs',
            ],
            // Include any special characters you're using in this regular expression
            defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || []
        }),
        require('autoprefixer'),
        require('postcss-nested')
    ]
}