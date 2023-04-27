#include <SoftwareSerial.h>
#include <SerialCommand.h>
SerialCommand sCmd;

const int pingPin = 7; // Trigger Pin of Ultrasonic Sensor
const int echoPin = 6; // Echo Pin of Ultrasonic Sensor
int distance = 5000;
void setup() {
   Serial.begin(9600); // Starting Serial Terminal
   while (!Serial);
   sCmd.addCommand("REQUESTDISTANCE", sendDistance);
}

void loop() {
   long duration, inches, cm;
   pinMode(pingPin, OUTPUT);
   digitalWrite(pingPin, LOW);
   delayMicroseconds(2);
   digitalWrite(pingPin, HIGH);
   delayMicroseconds(10);
   digitalWrite(pingPin, LOW);
   pinMode(echoPin, INPUT);
   duration = pulseIn(echoPin, HIGH);
   inches = microsecondsToInches(duration);
   cm = microsecondsToCentimeters(duration);
   //Serial.print(inches);
   //Serial.print("in, ");
   Serial.print(cm);
   //Serial.print("cm");
   Serial.println();
  
  distance = cm;

   delay(100);
}

long microsecondsToInches(long microseconds) {
   return microseconds / 74 / 2;
}

long microsecondsToCentimeters(long microseconds) {
   return microseconds / 29 / 2;
}

void sendDistance(){
  writeToUnity(distance);
}

void writeToUnity(int sensorValue){
  String valueString = "";
  valueString.concat(sensorValue);
  Serial.println(valueString);
  Serial.flush();
  delay(50);
}