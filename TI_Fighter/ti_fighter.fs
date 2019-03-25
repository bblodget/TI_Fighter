FORGET -->>

: -->> 0 EMIT [COMPILE] --> ; IMMEDIATE

: Reload ( --)
\ Reloads FIGHTER block file
S" DSK3.FIGHTER" USE
;

   0 VALUE Column			\ used by DrawIt
   0 VALUE Row			\ used by DrawIt
   1 CONSTANT Fire?		\ comparison check for fire button
   2 CONSTANT Left?		\ comparison check for left
   4 CONSTANT Right?		\ comparison check for right
   8 CONSTANT Down?		\ comparison check for down
  16 CONSTANT Up?			\ comparison check for up
  13 CONSTANT ENTER		\ key code for ENTER key
 400 CONSTANT DelayTime	\ delay loop count

HEX
\ user defined graphics
: BrickUDG ( --) \ character data for brick wall
   DATA 4 7FAA D5A8 D0A0 C080 80 DCHAR
   DATA 4 8080 8080 8080 807F 81 DCHAR
   DATA 4 FEA9 4101 0101 0101 82 DCHAR
   DATA 4 0101 0101 0101 01FE 83 DCHAR ;

: ManRight1 ( --) \ man facing right, frame #1
   DATA 4 0302 0201 0302 0607 100 DCHAR
   DATA 4 0603 0202 0202 0203 101 DCHAR
   DATA 4 0080 8000 C048 6850 102 DCHAR
   DATA 4 40C0 4040 4040 4070 103 DCHAR ;

: ManRight2 ( --) \ man facing right, frame #2
   DATA 4 0101 0100 0306 0A07 100 DCHAR
   DATA 4 0203 0202 0204 0406 101 DCHAR
   DATA 4 8060 6080 C868 5040 102 DCHAR
   DATA 4 40C0 4020 1010 101C 103 DCHAR ;

DECIMAL

: NextColumn ( --)	\ advance to the next column
   2 +TO Column ;

: DrawIt ( a b c d --)
  \ emits the four ascii characters on the stack to the screen as follows:
  \ ac
  \ bd
   Column 1+ Row 1+ GOTOXY EMIT \ d
   Column 1+ Row GOTOXY EMIT    \ c
   Column Row 1+ GOTOXY EMIT    \ b
   Column Row GOTOXY EMIT       \ a
   NextColumn ;

: DrawBrick ( --) \ draws a brick tile at Column & Row
   128 129 130 131 DrawIt ;

: ManSprite ( --) \  Defines sprite 0
    0 128 48 0 5 SPRITE ;

: DefineGraphics ( --)
  \ define the user defined graphics and set colours of character sets
   1 GMODE FALSE SSCROLL !
   32 0 DO I 1 14 COLOR LOOP
   16 1 15 COLOR  	\ brick colour
   BrickUDG ManRight1
   ;

: BrickRows ( --)
  \ draws rows of bricks at top and bottom of screen
   0 TO Row 
   2 0 DO 
     0 TO Column
     2 0 DO 
     16 0 DO DrawBrick LOOP 
       2 +TO Row 0 TO Column
     LOOP 
     20 TO Row
   LOOP ;

: Fighter ( --)
    \ entry point of game
    DefineGraphics BrickRows
    3 MAGNIFY
    ManSprite
    30 23 GOTOXY KEY DROP ;

    

