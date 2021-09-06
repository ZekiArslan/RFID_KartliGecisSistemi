#include <RFID.h>
#include <SPI.h>
#include <Servo.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>
LiquidCrystal_I2C lcd(0x27, 16, 2);

RFID rfidOkuyucu(10,9);
Servo servo;
int kartKontrol;
int kirmizi1=A1;
int kirmizi2=A2;
int kirmizi3=A3;
int yesil1=A0;
int yesil2=3;
int yesil3=4;

int buzzer= 2;
int donguKontrol=0;
int lcdKontrol=0;
String satir1,satir2;
boolean stringKontrol =false;
int altsatir=0;
int sayac=0;

void setup() {
  Serial.begin(9600);
  SPI.begin();           
  rfidOkuyucu.init();
  servo.attach(5);
  servo.write(180);
  lcd.begin();
  pinMode(kirmizi1,OUTPUT);
  pinMode(kirmizi2,OUTPUT);
  pinMode(kirmizi3,OUTPUT);
  
  pinMode(buzzer,OUTPUT);
  lcd.setCursor(0,0);
  lcd.println("  ZEKI ARSLAN   ");
  lcd.setCursor(0,1);
  lcd.println(" VERI ILETISIMI  ");
  digitalWrite(kirmizi1,HIGH);
  digitalWrite(yesil1,HIGH);
  delay(300);
  digitalWrite(kirmizi2,HIGH);
  digitalWrite(yesil2,HIGH);
  delay(300);
  digitalWrite(kirmizi3,HIGH);
  digitalWrite(yesil3,HIGH);
  delay(300);
  digitalWrite(kirmizi1,LOW);
  digitalWrite(kirmizi2,LOW);
  digitalWrite(kirmizi3,LOW);
  digitalWrite(yesil1,LOW);
  digitalWrite(yesil2,LOW);
  digitalWrite(yesil3,LOW);
  lcd.clear();
}

void loop() {
  lcd.setCursor(0,0);
  lcd.println("   GIRIS ICIN   ");
  lcd.setCursor(0,1);
  lcd.println("KARTINIZI OKUTUN");
  digitalWrite(yesil1,HIGH);
  digitalWrite(yesil2,HIGH);
  digitalWrite(yesil3,HIGH);
  donguKontrol=0;

  while(rfidOkuyucu.isCard()){
    if(donguKontrol==0){      
      if (rfidOkuyucu.readCardSerial()) {      
      Serial.println(rfidOkuyucu.serNum[0]);      
      Serial.print(rfidOkuyucu.serNum[1]);      
      Serial.print(rfidOkuyucu.serNum[2]);      
      Serial.print(rfidOkuyucu.serNum[3]); 
      Serial.print(rfidOkuyucu.serNum[4]);
      rfidOkuyucu.halt();
      lcd.setCursor(0,0);
      lcd.println("   KART OKUNDU   ");
      lcd.setCursor(0,1);
      lcd.println("  KARTINIZI ALIN ");
      digitalWrite(buzzer,HIGH);
      digitalWrite(kirmizi1,HIGH);
      digitalWrite(kirmizi2,HIGH);
      digitalWrite(kirmizi3,HIGH);
      delay(500);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      }
      delay(2000);
      }      
    }
    donguKontrol=1;
    lcdKontrol=0;
    delay(100);
    SeriHaberlesmeler();

  }


void SeriHaberlesmeler(){
  
    while(Serial.available()){
      if(lcdKontrol==0){
        lcd.clear();
        lcdKontrol=1;
        }

        

    char gelenKarakter=(char)Serial.read();

    if(gelenKarakter=='+'){   // Kapı açma komutu.
      digitalWrite(buzzer,HIGH);
      digitalWrite(kirmizi1,HIGH);
      digitalWrite(kirmizi2,HIGH);
      digitalWrite(kirmizi3,HIGH);
      delay(200);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      delay(200);
      digitalWrite(buzzer,HIGH);
      digitalWrite(kirmizi1,HIGH);
      digitalWrite(kirmizi2,HIGH);
      digitalWrite(kirmizi3,HIGH);
      delay(200);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      delay(200);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      servo.write(90);
      delay(200);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      stringKontrol =false;
      }

      if(gelenKarakter=='-'){ //GİRİS REDDİ
      digitalWrite(buzzer,HIGH);
      digitalWrite(kirmizi1,HIGH);
      digitalWrite(kirmizi2,HIGH);
      digitalWrite(kirmizi3,HIGH);
      delay(100);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      delay(100);
      digitalWrite(buzzer,HIGH);
      digitalWrite(kirmizi1,HIGH);
      digitalWrite(kirmizi2,HIGH);
      digitalWrite(kirmizi3,HIGH);
      delay(100);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      delay(100);
      digitalWrite(buzzer,HIGH);
      digitalWrite(kirmizi1,HIGH);
      digitalWrite(kirmizi2,HIGH);
      digitalWrite(kirmizi3,HIGH);
      delay(100);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      delay(100);
      digitalWrite(buzzer,HIGH);
      digitalWrite(kirmizi1,HIGH);
      digitalWrite(kirmizi2,HIGH);
      digitalWrite(kirmizi3,HIGH);
      delay(100);
      digitalWrite(buzzer,LOW);
      digitalWrite(kirmizi1,LOW);
      digitalWrite(kirmizi2,LOW);
      digitalWrite(kirmizi3,LOW);
      delay(100);
      stringKontrol =false;
        }
        
      if(gelenKarakter=='/') // ALT SATIRA GEÇ.
        {
            altsatir=1;
        }
    if(stringKontrol ==false && altsatir==0) //YAZI YAZ.
      {
           satir1 += gelenKarakter;
      }
    if(stringKontrol ==false && altsatir==1) //YAZI YAZ.
     {
           satir2 += gelenKarakter;      
     }
    sayac++;
  }
       altsatir=0;
       stringKontrol=false;
       lcd.setCursor(0,0);
       lcd.print(satir1);
       lcd.setCursor(0,1);
       lcd.print(satir2);
       Bitir();
  }


  void Bitir(){
  delay(2000);
  servo.write(180);
  
  satir1="";
  satir2="";
  int altsatir=0;
  int sayac=0;
  lcd.clear(); 
  
 }
