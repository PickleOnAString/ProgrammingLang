class Test {
    public Float number = 500;
    
    public class Foo {
        Float bar = 2000;
        
        public class Bar {
            private Float ob = 9000;
        }
    }
}

Float test = new Test().number;
test*100