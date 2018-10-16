<template>
    <div id="app">
        <h1>VueJS App</h1>
        <div>Hello World {{ title }} - {{ count }}</div>
        <button @click="call()">add to count</button>
    </div>
</template>

<script>
    export default {
        name: 'app',
        data () {
            return {
                count: 0,
            }
        },
        methods: {
            call() {
                if (!this.count) {
                    this.count = 0;
                }
                this.count++;
            }
        },
        computed: {
            title() {
                return this.$store.state.title;
            }
        }
    }
</script>
