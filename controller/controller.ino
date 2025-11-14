int buttonPin1 = 7, buttonPin2 = 8, buttonPin3 = 6, buttonPin4;
int curState1, curState2, curState3, curState4;
int prevState1 = 0, prevState2 = 0, prevState3 = 0, prevState4 = 0;

void setup() {
  pinMode(buttonPin1, INPUT);
  pinMode(buttonPin2, INPUT);
  pinMode(buttonPin3, INPUT);
  Serial.begin(9600);
}

void press(int buttonPin, int care, int &curState, int &prevState)
{
  curState = digitalRead(buttonPin);
  if(curState != prevState){
    if(curState == 1){
      Serial.println("Button " + (String)care + " pressed");
    } 
  }
  prevState=curState;
}

void loop() {
  press(buttonPin1, 1, curState1, prevState1);
  press(buttonPin2, 2, curState2, prevState2);
  press(buttonPin3, 3, curState3, prevState3);
}
