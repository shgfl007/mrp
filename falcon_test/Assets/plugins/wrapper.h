#ifndef WRAPPER_H
#define WRAPPER_H

//////////////////////////////////////////////////////////////////////
// engine functions
#define EXT	extern "C"	// prevent VC++ Linker from adding a "?" to C++ functions
#define DLLFUNC extern "C" __declspec(dllexport)

#ifdef DLL_USE
#define F(f) (*f)	// use function pointers in a DLL
#else
#define F(f) f		// use function prototypes in a EXE
#endif

#undef F
#undef EXT

//////////////////////////////////////////////////////////////////////
// structs

typedef struct LINK {
	long	index;		// index number of the object
	struct LINK *next; // pointer to next object
} LINK;


typedef struct STRING {
	char	*chars;		// pointer to null terminated string
	long	length;		// allocated length of string (NEVER exceed!)
} STRING;

//////////////////////////////////////////////////////////////////////
// conversion macros
inline long _LONG(int i) { return i<<10; }					// int -> var
inline long _LONG(double f) { return (long)(f*(1<<10)); }		// double -> var, overloaded
inline int _INT(long x) { return x>>10; }					// var -> int
inline float _FLOAT(long x) { return ((float)x)/(1<<10); }	// var -> float
inline char* _CHR(STRING* s) { return s->chars; }			// STRING* -> char*



#endif