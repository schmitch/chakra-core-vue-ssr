<template>
    <div>Hello World {{ title }}</div>
</template>

<script>
    export default {
        name: 'app',
        computed: {
            title () {
                return this.$store.state.title;
            }
        }
    }
</script>
