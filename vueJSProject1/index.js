const app = Vue.createApp({
    //data, function
    data(){
        return {
            showBook: true,
            title : 'Breadth Firsth Search',
            author: 'Dennis Ritchi',
            age: 50,
            x: 0,
            y: 0
        }
    },
    methods: {
        changeTitle(title){
            //this.title = 'The Alchemists'
            this.title = title
        },
        hideBooks(){
            this.showBook = !this.showBook
        },
        handleEvent(event, data){
            console.log(event, event.type)
            if(data){
                console.log(data)
            }
        },
        handleMouseMove(event){
            this.x = event.offsetX
            this.y = event.offsetY
        }
    }
})

app.mount("#app")
